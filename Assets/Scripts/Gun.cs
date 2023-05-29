using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] private Transform playerCam;

    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip switchSound;
    public Animator animator;

    public GameObject muzzleFlash;
    public Transform muzzle;
    public BulletHolePoolNew bulletHolePoolNew;
    private float timeSinceLastShot;
    public Recoil recoil;
    private bool canShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponManager.curAmmo = gunData.currentAmmo;
        PlayerController.shootInput += Shoot;
        PlayerController.reloadInput += StartReload;
        //gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
        animator.SetBool("isFiring", false);
        audioSource.PlayOneShot(switchSound);
    }
    public void Shoot()
    {
        if (gunData.currentAmmo <= 0)
        {
            gunData.currentAmmo = 0;
            PlayerController.noAmmo = true;
            StartCoroutine(Reload());
            return;
        }
        if (gunData.currentAmmo > 0)
        {
            if (canShoot())
            {
                int layerMask = 1 << 9;
                layerMask = ~layerMask;
                if (Physics.Raycast(playerCam.position, playerCam.transform.forward, out RaycastHit hitInfo, gunData.maxDistance,layerMask))
                {
                    //Instantiate(bulletHole, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
                    bulletHolePoolNew.CreateBulletHole(hitInfo.point, hitInfo.collider.gameObject, hitInfo.normal);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.weaponDamage);
                    Debug.Log(hitInfo.collider.name);
                }
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
        //PlayerController.adsInput -= ADS;
        PlayerController.shootInput -= Shoot;
        PlayerController.reloadInput -= StartReload;
        gunData.reloading = false;
        StopAllCoroutines();
    }

}
