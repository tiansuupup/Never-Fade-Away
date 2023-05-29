using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] private Transform playerCam;
    [SerializeField] private GameObject weaponCam;
    public GameObject scope;
    public GameObject crossHair;
    public Animator animator;

    private AudioSource audioSource;

    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip switchSound;
    public AudioClip scopeSound;

    private int zoomLevel = 0;
    private bool scopeable;
    private float timeSinceLastShot;
    public Recoil recoil;

    public GameObject muzzleFlash;
    public BulletHolePoolNew bulletHolePoolNew;
    public Transform muzzle;
    private Vector3 shootDirection;
    private bool isScoped;
    private bool canShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    private void OnEnable()
    {
        isScoped = false;
        crossHair.SetActive(false);
        scopeable = false;
        timeSinceLastShot = 10.0f;
        audioSource = GetComponent<AudioSource>();
        //gunData.currentAmmo = gunData.magSize;
        WeaponManager.curAmmo = gunData.currentAmmo;

        zoomLevel = 0;
        PlayerController.adsInput += ADS;
        PlayerController.shootInput += Shoot;
        PlayerController.reloadInput += StartReload;
        gunData.reloading = false;
        Invoke("OpenScope", 0.25f);
        animator.SetBool("isFiring", false);
        audioSource.PlayOneShot(switchSound);
    }

    public void OpenScope()
    {
        if (gunData.currentAmmo > 0)
        {
            scopeable = true;
        }
    }

    public void ADS()
    {
        if (!canShoot())
        {
            return;
        }
        if (!scopeable)
        {
            return;
        }
        zoomLevel++;
        if (zoomLevel == 1)
        {
            isScoped = true;
            audioSource.PlayOneShot(scopeSound);
            playerCam.GetComponent<Camera>().fieldOfView = 40;
            weaponCam.SetActive(false);
            scope.SetActive(true);
        }
        if (zoomLevel == 2)
        {
            isScoped = true;
            audioSource.PlayOneShot(scopeSound);
            playerCam.GetComponent<Camera>().fieldOfView = 20;
            weaponCam.SetActive(false);
            scope.SetActive(true);
        }
        if (zoomLevel == 3)
        {
            isScoped = false;
            playerCam.GetComponent<Camera>().fieldOfView = 60;
            weaponCam.SetActive(true);
            scope.SetActive(false);
            zoomLevel = 0;
        }

    }

    public void ResetSniper() 
    {
        playerCam.GetComponent<Camera>().fieldOfView = 60;
        weaponCam.SetActive(true);
        scope.SetActive(false);
        zoomLevel = 0;
        isScoped = false;
    }


    public void Shoot()
    {
        if (gunData.currentAmmo <= 0)
        {
            gunData.currentAmmo = 0;
            PlayerController.noAmmo = true;
            return;
        }
        if (gunData.currentAmmo > 0)
        {
            if (canShoot())
            {
                int layerMask = 1 << 9;
                layerMask = ~layerMask;
                animator.SetTrigger("gunFire");

                float xRandom = Random.Range(0.2f, 5f);
                float yRandom = Random.Range(0.3f, 3f);
                float zRandom = Random.Range(0.5f, 1.5f);
                Vector3 unScoppedShot = new Vector3(playerCam.transform.forward.x * xRandom, playerCam.transform.forward.y * yRandom, playerCam.transform.forward.z * zRandom);

                if (isScoped)
                {
                    shootDirection = playerCam.transform.forward;
                }
                else
                {
                    shootDirection = unScoppedShot.normalized;

                }
                Debug.Log(shootDirection);

                //if (Physics.Raycast(playerCam.position, playerCam.transform.forward, out RaycastHit hitInfo, gunData.maxDistance,layerMask))
                if (Physics.Raycast(playerCam.position, shootDirection, out RaycastHit hitInfo, gunData.maxDistance, layerMask))
                {
                    bulletHolePoolNew.CreateBulletHole(hitInfo.point, hitInfo.collider.gameObject, hitInfo.normal);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.weaponDamage);
                }
                ResetSniper();
                recoil.RecoilFire();
                gunData.currentAmmo--;

                WeaponManager.curAmmo = gunData.currentAmmo;

                timeSinceLastShot = 0;
                OnGunShot();

            }
        }
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        animator.SetBool("isFiring", false);
        audioSource.PlayOneShot(reloadSound);

        ResetSniper();
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
        PlayerController.noAmmo = false;
        WeaponManager.curAmmo = gunData.currentAmmo;
        timeSinceLastShot = 0f;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (gunData.currentAmmo >= gunData.magSize)
        {
            PlayerController.fullAmmo = true;
        }
        else
        {
            PlayerController.fullAmmo = false;
        }
    }
    private void OnGunShot()
    {
        Instantiate(muzzleFlash, muzzle.position, Quaternion.identity);
        audioSource.PlayOneShot(shootSound);
    }


    private void OnDisable()
    {
        isScoped = false;
        crossHair.SetActive(true);
        zoomLevel = 0;
        PlayerController.adsInput -= ADS;
        PlayerController.shootInput -= Shoot;
        PlayerController.reloadInput -= StartReload;
        gunData.reloading = false;
        StopAllCoroutines();
    }

}
