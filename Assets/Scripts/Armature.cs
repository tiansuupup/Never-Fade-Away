using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Armature : MonoBehaviour
{
    public GameObject myPlayer;
    private Animator myAnimator;
    public GameObject leftHandTarget;
    public GameObject gunTarget;

    public GameObject primaryWeapon;
    public FullBodyBipedIK ik;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void SwitchToHand()
    {
        if (WeaponManager.primaryInt == 1)
        {
            ik.solver.leftHandEffector.target = leftHandTarget.transform.GetChild(0).transform;
        }
        if (WeaponManager.primaryInt == 2)
        {
            ik.solver.leftHandEffector.target = leftHandTarget.transform.GetChild(1).transform;
        }
    }

    public void FireFinish()
    {
        PlayerController.fireFinihed = true;
    }

    public void FinishSwitch()
    {
        PlayerController.duringSwitchAnimation = false;
        myAnimator.SetBool("isSwitching", false);
    }

    public void FinishReload()
    {
        if (WeaponManager.primaryInt == 1)
        {
            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(0).transform.GetChild(0).transform;
        }
        if (WeaponManager.primaryInt == 2)
        {
            ik.solver.leftHandEffector.target = primaryWeapon.transform.GetChild(1).transform.GetChild(0).transform;
        }

        //ik.solver.leftHandEffector.target = gunTarget.transform;
        myPlayer.GetComponent<PlayerController>().duringReloadAnimation = false;
        myAnimator.SetBool("isReloading", false);

    }
}
