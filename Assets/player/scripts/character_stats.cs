using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_stats : MonoBehaviour
{
    [Header("Character in-game stats")]
    public float health;
    public float walking_speed;
    public float run_speed;
    public float jump_speed;
    public float damage;
    public float money;
    public float maxAmmo;

    public bool isReloading;
    public bool isShielded;
    public bool isHealing;

    [HideInInspector]
    public float base_health;
    [HideInInspector]
    public float base_walking_speed;
    [HideInInspector]
    public float base_run_speed;
    [HideInInspector]
    public float base_damage;



    //is armour and weapon equipped?
    [HideInInspector]
    public bool equiped_weapon;
    [HideInInspector]
    public bool equiped_armour;

    //if armour and weapon are equiped they are stored here for later reference
    [HideInInspector]
    public GameObject equiped_weapon_object;
    [HideInInspector]
    public GameObject equiped_chest_armour_object;


    void Start()
    {
        equiped_weapon = false;
        equiped_armour = false;

        base_health = health;
        base_damage = damage;
        base_walking_speed = walking_speed;
        base_run_speed = run_speed;

        isReloading = false;
        isShielded = false;
        isHealing = false;
    }
}
