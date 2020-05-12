using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class gameLogic : MonoBehaviour
{
    public int round;

    public GameObject enemyPrefab;
    public GameObject startButton;
    public GameObject winScreen;
    public GameObject roundStartScreen;

    GameObject player;
    GameObject camera;
    int numberOfEnemiesBeaten;
    bool roundInProgress;

    public GameObject[] spawnPoints;
    public GameObject spawnedEnemies;

    Dictionary<int, GameObject> enemies;

    public int numberOfEnemies = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        enemies = new Dictionary<int, GameObject>();
        round = 0;
        roundInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        //player hit the button and started the round
        if (startButton.GetComponent<startButton>().startRound && !roundInProgress)
            spawnEnemies();
    }

    void spawnEnemies()
    {
        roundInProgress = true;
        startButton.GetComponent<startButton>().startRound = false;
        for (int i = 0; i < numberOfEnemies; i++)
            enemies.Add(i, Instantiate(enemyPrefab, spawnPoints[i].transform.position, new Quaternion(), spawnedEnemies.transform));
        round += 1;

        roundStartScreen.transform.Find("Round Number").GetComponent<Text>().text = round.ToString();
        roundStartScreen.transform.Find("Enemies Number").GetComponent<Text>().text = numberOfEnemies.ToString();

        roundStartScreen.SetActive(true);

    }

    public void despawnEnemy()
    {
        numberOfEnemiesBeaten += 1;
        if(numberOfEnemies == numberOfEnemiesBeaten)
            Invoke("PlayerWon", 2);

    }

    private void PlayerWon()
    {
        //player won the round
        roundInProgress = false;
        roundStartScreen.SetActive(false);

        foreach (GameObject enemy in enemies.Values)
            Destroy(enemy);
        enemies.Clear();
        numberOfEnemiesBeaten = 0;

        winScreen.SetActive(true);
        player.GetComponent<character_controller>().enabled = false;
        camera.GetComponent<mouse_aim>().enabled = false;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 150f;

        winScreen.transform.Find("Round stats").Find("Enemies beaten").Find("Number").GetComponent<Text>().text = numberOfEnemies.ToString();
        winScreen.transform.Find("Round stats").Find("Money won").Find("Number").GetComponent<Text>().text = (numberOfEnemies * 100f).ToString();

        player.GetComponent<character_stats>().money += numberOfEnemies * 100f;

    }

    public void nextRound()
    {
        winScreen.SetActive(false);
        player.GetComponent<character_controller>().enabled = true;
        camera.GetComponent<mouse_aim>().enabled = true;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 0f;
    }
}
