using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_actions_manager : MonoBehaviour
{
    private bool isReloading = false;
    private bool isBeingHit = false;
    private bool isHealing = false;

    private float damageRecieved;

    private character_stats characterStats;

    void Start()
    {
        characterStats = transform.GetComponent<character_stats>();
    }

    void Update()
    {
        if (isReloading && characterStats.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo <= characterStats.maxAmmo)
        {
            characterStats.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo += characterStats.equiped_weapon_object.GetComponent<weapon>().rateOfAmmoSpending * 3;
            characterStats.isReloading = true;
        }
        else
            characterStats.isReloading = false;

        if (characterStats.health < 0)
            characterStats.isShielded = true;
        else
            characterStats.isShielded = false;

        if (isBeingHit)
            characterStats.health += damageRecieved * Time.deltaTime;

        if (isHealing)
            if(characterStats.health > 0)
            characterStats.health -= 0.05f * Time.deltaTime;

        //if(characterStats.health <= 0)
        //{
        //    transform.GetComponent<Animator>().SetBool("is_beaten", true);
        //}
    }

    void OnTriggerStay(Collider collider)
    {
        //player is standing near water tap and is reloading weapon
        if (collider.CompareTag("waterTap") && characterStats.equiped_weapon)
        {
            isReloading = true;
            ParticleSystem water = collider.gameObject.transform.Find("water").GetComponent<ParticleSystem>();
            if (!water.isPlaying)
                water.Play();

        }
        //player is being attacked by enemy
        if (collider.CompareTag("enemy_weapon"))
        {
            isBeingHit = true;
            damageRecieved = collider.transform.parent.GetComponent<enemy_weapon>().Damage;
        }

        if (collider.CompareTag("fan"))
            isHealing = true;
            
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("waterTap") && characterStats.equiped_weapon)
        {
            isReloading = false;
            ParticleSystem water = collider.gameObject.transform.Find("water").GetComponent<ParticleSystem>();
            if (water.isPlaying)
                water.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (collider.CompareTag("enemy_weapon"))
            isBeingHit = false;
        if (collider.CompareTag("fan"))
            isHealing = false;
    }
}
