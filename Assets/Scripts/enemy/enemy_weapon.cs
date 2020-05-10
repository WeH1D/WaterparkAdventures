using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_weapon : MonoBehaviour
{
    public float CurrentAmmo;
    public float MaxAmmo;
    public float RateOfAmmoSpending;
    [Range(0.05f, 0.1f)]
    public float Damage;

}
