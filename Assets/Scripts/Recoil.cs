using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Header("Recoil Data")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [Header("Recoil Setting")]
    public float snappiness;
    public float returnSpeed;

    [Header("Weapon Recoil")]
    public bool weaponRecoilEnabled;
    private Vector3 multipliedRotation;
    public float recoilMultiplier;
    public GameObject armature;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        multipliedRotation = new Vector3(currentRotation.x * recoilMultiplier, currentRotation.y * recoilMultiplier, currentRotation.z * recoilMultiplier);
        transform.localRotation = Quaternion.Euler(currentRotation);
        if (weaponRecoilEnabled)
        {
            armature.transform.localRotation = Quaternion.Euler(multipliedRotation);
        }
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

    }
}
