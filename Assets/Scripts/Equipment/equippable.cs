using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equippable : MonoBehaviour
{ 
    //This script is shared between armour and weapon objects that can be equiped by the player
    public bool equiped;

    public GameObject player;
    public character_controller controller;
    public character_stats character_stats;

    public Sprite thumbnail;
    public float price;

    void Awake()
    {
        equiped = false;
        player = GameObject.FindGameObjectWithTag("player");
        controller = player.GetComponent<character_controller>();
        character_stats = player.GetComponent<character_stats>();
    }
}
