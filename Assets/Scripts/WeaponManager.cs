using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class WeaponManager : MonoBehaviour
{
    public int testInt;
    public static int primaryInt;
    public GameObject primaryWeapon;
    public FullBodyBipedIK ik;
    public Animator animator;

    public GameObject cameraRecoil;

    public static int curAmmo;
    public Text curAmmoText;


    void Start()
    {
        primaryInt = testInt;
        if (primaryInt == 1)
        {
            animator.SetFloat("fireSpeed", 1f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(true);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(false);

            curAmmo = (primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.currentAmmo);
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recMult;


            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(0).transform.GetChild(0).transform;
        }
        if (primaryInt == 2)
        {
            animator.SetFloat("fireSpeed", 0.1f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(true);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(false);

            curAmmo = (primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.currentAmmo);
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recMult;

            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(1).transform.GetChild(0).transform;
        }
        if (primaryInt == 3)
        {
            animator.SetFloat("fireSpeed", 0.7f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(true);

            curAmmoText.text = (primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.currentAmmo).ToString();
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recMult;

            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(2).transform.GetChild(0).transform;
        }

        curAmmoText.text = curAmmo.ToString();
    }

    public void SwitchGun()
    {
        if (primaryInt == 1)
        {
            animator.SetFloat("fireSpeed", 1f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(true);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(false);

            curAmmoText.text = (primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.currentAmmo).ToString();
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(0).gameObject.GetComponent<Gun>().gunData.recMult;

            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(0).transform.GetChild(0).transform;
        }
        if (primaryInt == 2)
        {
            animator.SetFloat("fireSpeed", 0.1f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(true);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(false);

            curAmmoText.text = (primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.currentAmmo).ToString();
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(1).gameObject.GetComponent<Sniper>().gunData.recMult;

            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(1).transform.GetChild(0).transform;
        }
        if (primaryInt == 3)
        {
            animator.SetFloat("fireSpeed", 0.7f);
            primaryWeapon.transform.GetChild(0).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(1).gameObject.SetActive(false);
            primaryWeapon.transform.GetChild(2).gameObject.SetActive(true);

            curAmmoText.text = (primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.currentAmmo).ToString();
            cameraRecoil.GetComponent<Recoil>().recoilX = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recX;
            cameraRecoil.GetComponent<Recoil>().recoilY = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recY;
            cameraRecoil.GetComponent<Recoil>().recoilZ = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recZ;
            cameraRecoil.GetComponent<Recoil>().returnSpeed = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.retSpeed;
            cameraRecoil.GetComponent<Recoil>().recoilMultiplier = primaryWeapon.transform.GetChild(2).gameObject.GetComponent<Pistol>().gunData.recMult;

            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(2).transform.GetChild(0).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        curAmmoText.text = curAmmo.ToString();
    }
}
