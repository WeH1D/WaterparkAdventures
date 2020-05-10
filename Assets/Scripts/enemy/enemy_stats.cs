using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_stats : MonoBehaviour
{
    [Range(.05f, .2f)]
    public float walk_speed;
    [Range(.2f, .4f)]
    public float run_speed;
    [Range(3,10)]
    public float rotateSpeed;
    public float health;

    public bool has_equiped_weapon;

    void Update()
    {
        if (transform.Find("Armature").Find("RightFistControl").Find("weapon_r").childCount == 0)
            has_equiped_weapon = false;
        else
            has_equiped_weapon = true;
    }
}
