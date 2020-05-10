using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapon : MonoBehaviour
{
    private equippable equippable;

    bool equiped;
    private GameObject player;
    private GameObject weaponHand;
    private character_controller controller;
    private GameObject AmmoBarUI;
    private character_stats character_stats;
    private float price;
    private ParticleSystem water;

    [Header("Weapon stats")]
    public float CurrentAmmo;
    public float MaxAmmo;
    public float rateOfAmmoSpending;
    [Range(0.05f, 0.1f)]
    public float damage;

    void Start()
    {
        equippable = transform.GetComponent<equippable>();

        equiped = equippable.equiped;
        player = equippable.player;
        controller = equippable.controller;
        price = equippable.price;
        character_stats = equippable.character_stats;
        water = transform.Find("water_stream").Find("water").GetComponent<ParticleSystem>();

        weaponHand = player.transform.Find("Armature").Find("RightFistControl").Find("weapon_r").gameObject;
        AmmoBarUI = GameObject.Find("UI").transform.Find("Ammo Bar").gameObject;
    }

    void OnTriggerEnter(Collider colision)
    {
        if (colision.transform.tag == "player") 
        {
           
            if (character_stats.equiped_weapon == false)
            {
                character_stats.equiped_weapon_object = transform.gameObject;

                character_stats.maxAmmo += MaxAmmo;

                AmmoBarUI.transform.Find("Slider").GetComponent<Slider>().maxValue = character_stats.maxAmmo;

                //Ignores players collider when its equiped so that it doesnt obstruct the player
                Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), player.GetComponent<BoxCollider>(), true);
               
                transform.GetComponent<Rigidbody>().useGravity = false;

                //Equipts the weapon and places it in players handS
                transform.parent = weaponHand.transform;

                //Enables all constraints of the weapon so that the weapon moves acording to players hand and not on its ownS
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                transform.Find("water_stream").transform.tag = "player_weapon";

                character_stats.equiped_weapon = true;
                equiped = true;
                
            }
        
        }

    }

    void Update()
    {
        if (equiped)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (CurrentAmmo > character_stats.maxAmmo)
                CurrentAmmo = character_stats.maxAmmo;
        }

        //to drop the weapon the player presses G
        if (Input.GetKeyDown(controller.Drop) && equiped == true)
        {
            equiped = false;
            transform.parent = null;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().velocity = -transform.forward * 20;
            character_stats.equiped_weapon = false;

            stopShooting();

            character_stats.maxAmmo -= MaxAmmo;

            //waits for a second to stop ignoring the players collider so that it has time to drop on the floor without the player automatically picking it up again
            Invoke("IgnorePlayerFalse", 1);
        }
    }

    void IgnorePlayerFalse()
    {
        Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), player.GetComponent<BoxCollider>(), false);
    }

    public void startShooting()
    {
        if (!water.isPlaying)
            water.Play();
    }

    public void stopShooting()
    {
        if (water.isPlaying)
            water.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
