using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class pauseScreen : MonoBehaviour
{
    public new GameObject camera;
    public character_controller controller;
    public GameObject screen;
    public GameObject helpScreen;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(controller.PauseScreen))
        {
            pauseScreenOpenClose();
            helpScreen.SetActive(false);
        }

    }

    public void pauseScreenOpenClose()
    {
        if (!screen.activeSelf)
        {
            screen.SetActive(true);
            controller.enabled = false;
            camera.GetComponent<mouse_aim>().enabled = false;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 150f;
        }
        else
        {
            screen.SetActive(false);
            controller.enabled = true;
            camera.GetComponent<mouse_aim>().enabled = true;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 0f;


        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openHelpScreen()
    {
        if (!helpScreen.activeSelf)
            helpScreen.SetActive(true);
        else
            helpScreen.SetActive(false);
    }
}
