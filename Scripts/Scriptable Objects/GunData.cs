using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Gun Properties")]
    public float damage;
    public float maxDistance;
    public float fireRate;
    public float recoilForce;

    

  
 


}
