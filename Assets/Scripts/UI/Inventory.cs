using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public character_controller controller;
    bool UI_isOpen = false;
    public GameObject menu;

    public new GameObject camera;
    public GameObject player;
    GameObject player_copy;

    public GameObject character_space;
    public GameObject weapon_space;
    public GameObject armour_space;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(controller.Inventory))
        {
            UI_isOpen = !UI_isOpen;
            if (UI_isOpen)
            {
                //Disables player controls and camera mouse aim script while menu is open as well as stops in game time
                player.GetComponent<character_controller>().enabled = false;
                Time.timeScale = 0;
                camera.GetComponent<mouse_aim>().enabled = !UI_isOpen;

                //Instantiates a copy of the player which is shown in the "inventory" menu and places it correctly
                player_copy = Instantiate(player, menu.transform.position,new Quaternion(), menu.transform);
                player_copy.transform.LookAt(camera.transform);
                player_copy.transform.Rotate(new Vector3(0, -20, 0));
                player_copy.transform.localScale = new Vector3(125, 125, 125);
                player_copy.transform.localPosition += new Vector3(0, -270, 0);
                player_copy.layer = 5; // UI layer


                //Show sprites of currently equiped gear in the inventory menu
                if (player.GetComponent<character_stats>().equiped_weapon)
                    weapon_space.GetComponent<Image>().sprite = player.GetComponent<character_stats>().equiped_weapon_object.GetComponent<equippable>().thumbnail;

                if (player.GetComponent<character_stats>().equiped_armour)
                    armour_space.GetComponent<Image>().sprite = player.GetComponent<character_stats>().equiped_chest_armour_object.GetComponent<equippable>().thumbnail;

            }
            else
            {
                //reanables everything that was disabled and deletes player copy once the inventory is closed
                player.GetComponent<character_controller>().enabled = true;
                Destroy(GameObject.Find("warior(Clone)"));
                Time.timeScale = 1;
                GameObject.Find("Main Camera").GetComponent<mouse_aim>().enabled = !UI_isOpen;

                weapon_space.GetComponent<Image>().sprite = null;
                armour_space.GetComponent<Image>().sprite = null;
            }
            menu.SetActive(UI_isOpen);
        }
    }
}
