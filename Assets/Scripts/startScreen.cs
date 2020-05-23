using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class startScreen : MonoBehaviour
{
    public GameObject startMenu;
    new public GameObject camera;
    public GameObject player;
    public GameObject healthBar;
    public GameObject ammoBar;
    public GameObject money;
    public character_controller controller;

    pauseScreen PauseScreen;
    public GameObject helpScreen;


    void Start()
    {
        startMenu.SetActive(true);

        PauseScreen = transform.GetComponent<pauseScreen>();
        PauseScreen.enabled = false;

        healthBar.SetActive(false);
        ammoBar.SetActive(false);
        money.SetActive(false);

        player.GetComponent<character_controller>().enabled = false;
        camera.GetComponent<mouse_aim>().enabled = false;

        camera.GetComponent<Animation>().Play();
        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 100f;
    }

    void Update()
    {
        if (helpScreen.activeSelf && Input.GetKeyDown(controller.PauseScreen))
            helpScreen.SetActive(false);
            

    }

    public void startPressed()
    {
        startMenu.SetActive(false);

        PauseScreen.enabled = true;

        healthBar.SetActive(true);
        ammoBar.SetActive(true);
        money.SetActive(true);

        player.GetComponent<character_controller>().enabled = true;
        camera.GetComponent<mouse_aim>().enabled = true;

        camera.GetComponent<Animation>().Stop();
        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 0f;
    }

    public void exitPressed()
    {
        Application.Quit();
    }
    public void helpPressed()
    {
        helpScreen.SetActive(true);
    }

}
