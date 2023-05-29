using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Shooting")]
    public float weaponDamage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public bool reloading;

    [Header("Recoil Data")]
    public float recX;
    public float recY;
    public float recZ;
    public float retSpeed;
    public float recMult;


}
