using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class InterractUI: MonoBehaviour
{
   


    public GameObject player;
    public new GameObject camera;
    public character_controller controller;
    public GameObject UI;
    private bool UIOpen;
    private bool inRange;
  
    void Start()
    {
        UIOpen = false;
    }

    void Update()
    {
        if(inRange)
        {
            if(Input.GetKeyDown(controller.Interact))
            {
                UIActivation(); 
            }
        }
    }

    public void UIActivation()
    {
        UIOpen = !UIOpen;
        if (UIOpen)
            openUI();
        else
            closeUI();
    }

    private void openUI()
    {
        //Disables player controls and camera mouse aim script while menu is open as well as stops in game time
        player.GetComponent<character_controller>().enabled = false;
        Time.timeScale = 0;
        camera.GetComponent<mouse_aim>().enabled = false;

        UI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 150f;

    }

    private void closeUI()
    {
        //Enables player controls and camera mouse aim script while menu is open as well as stops in game time
        player.GetComponent<character_controller>().enabled = true;
        Time.timeScale = 1;
        camera.GetComponent<mouse_aim>().enabled = true;

        UI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 0f;

    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("player"))
        {
            inRange = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("player"))
        {
            inRange = false;
        }
    }
}
