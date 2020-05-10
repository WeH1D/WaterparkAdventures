using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armour : MonoBehaviour
{
    private equippable equippable;
    bool equiped;

    private GameObject player;
    private GameObject PlayerDeafultTorso;
    private character_controller controller;
    private float price;
    private Sprite thumbnail;
    
    private GameObject torsoLowerPlayer;
    private GameObject torsoUpperPlayer;
    private GameObject torsoUpperArmour;
    private GameObject torsoLowerArmour;
    private character_stats player_stats;

    //bonus stats that the armour gives
    public float bonus_speed;
    public float bonus_health;
    public float bonus_damage;
    public float bonus_water;

    void Start()
    {
        equippable = transform.GetComponent<equippable>();

        player = equippable.player;
        controller = equippable.controller;
        player_stats = equippable.character_stats;
        price = equippable.price;
        equiped = equippable.equiped;

        PlayerDeafultTorso = player.transform.Find("Torso").gameObject;

        torsoLowerArmour = transform.Find("Armature").Find("TorsoLowerArmour").gameObject;
        torsoUpperArmour = transform.Find("Armature").Find("TorsoLowerArmour").Find("TorsoUpperArmour").gameObject;

        torsoUpperPlayer = player.transform.Find("Armature").Find("LowerChestBone").Find("UpperChestBone").gameObject;
        torsoLowerPlayer = player.transform.Find("Armature").Find("LowerChestBone").gameObject;


    }

    void OnTriggerEnter(Collider colision)
    {
        if (colision.transform.tag == "player")
        {
            //Enables all constraints of the weapon so that the weapon moves acording to players hand and not on its ownS
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            if (player_stats.equiped_armour == false)
            {
                //stores the equiped armour object for later reference
                player_stats.equiped_chest_armour_object = transform.gameObject;
                player_stats.equiped_armour = true;

                //Ignores players collider when its equiped so that it doesnt obstruct the player
                Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), player.GetComponent<BoxCollider>(), true);

                transform.GetComponent<Rigidbody>().useGravity = false;

                //Equipts the armour
                transform.parent = torsoLowerPlayer.transform;

                transform.localPosition = new Vector3(0, 0, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 90);

                PlayerDeafultTorso.SetActive(false);

                //gives bonus stats to player according to the gear it wears
                player_stats.health += bonus_health;
                player_stats.walking_speed += bonus_speed;
                player_stats.run_speed += bonus_speed;
                player_stats.damage += bonus_damage;
                player_stats.maxAmmo += bonus_water;

                equiped = true;
            }

        }

    }

    void Update()
    {
        if (equiped)
        {
            //Makes the armour follow animated bones of the player when moving
            torsoUpperArmour.transform.position = torsoUpperPlayer.transform.position;
            torsoUpperArmour.transform.rotation = torsoUpperPlayer.transform.rotation;

            torsoLowerArmour.transform.position = torsoLowerPlayer.transform.position;
            torsoLowerArmour.transform.rotation = torsoLowerPlayer.transform.rotation;
        }

        //to drop the weapon the player presses G
        if (Input.GetKeyDown(controller.Drop) && equiped == true)
        {
            equiped = false;
            transform.parent = null;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().velocity = -transform.forward * 20;
            player.GetComponent<character_stats>().equiped_armour = false;
            PlayerDeafultTorso.SetActive(true);

            //takes away the bonus stats when gear is taken off
            player_stats.health -= bonus_health;
            player_stats.walking_speed -= bonus_speed;
            player_stats.run_speed -= bonus_speed;
            player_stats.damage -= bonus_damage;
            player_stats.maxAmmo -= bonus_water;


            //waits for a second to stop ignoring the players collider so that it has time to drop on the floor without the player automatically picking it up again
            Invoke("IgnorePlayerFalse", 1);
        }
    }
    void IgnorePlayerFalse()
    {
        Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), GameObject.FindGameObjectWithTag("player").GetComponent<BoxCollider>(), false);
    }
}
