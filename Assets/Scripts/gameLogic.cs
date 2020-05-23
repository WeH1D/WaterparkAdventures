using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class gameLogic : MonoBehaviour
{
    public int round;
    int roundsWon = 0;

    public GameObject enemyPrefab;
    public GameObject startButton;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject roundStartScreen;

    GameObject player;
    new GameObject camera;
    int numberOfRoundEnemiesBeaten;
    int numberOfAllEnemiesBeaten;
    bool roundInProgress;
    bool gameOverIsOpen;

    public GameObject[] spawnPoints;
    public GameObject spawnedEnemies;

    Dictionary<int, GameObject> enemies;

    int numberOfEnemies;

    int previousEnemiesBeaten;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        enemies = new Dictionary<int, GameObject>();
        //round = 0;
        roundInProgress = false;
        gameOverIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        //player hit the button and started the round
        if (startButton.GetComponent<startButton>().startRound && !roundInProgress)
        {
            round += 1;
            numberOfEnemies = (round / 3) + 1;
            spawnEnemies();
        }

        if (player.GetComponent<character_actions_manager>().hasLost && !gameOverIsOpen)
        {
            gameOverIsOpen = true;
            player.GetComponent<character_controller>().enabled = false;
            camera.GetComponent<mouse_aim>().enabled = false;
            Invoke("gameOver", 2);
        }
    }

    void spawnEnemies()
    {
        roundInProgress = true;
        startButton.GetComponent<startButton>().startRound = false;
        for (int i = 0; i < numberOfEnemies; i++)
            enemies.Add(i, Instantiate(enemyPrefab, spawnPoints[i].transform.position, new Quaternion(), spawnedEnemies.transform));

        roundStartScreen.transform.Find("Round Number").GetComponent<Text>().text = round.ToString();
        roundStartScreen.transform.Find("Enemies Number").GetComponent<Text>().text = numberOfEnemies.ToString();

        roundStartScreen.SetActive(true);

    }

    public void despawnEnemy()
    {
        numberOfRoundEnemiesBeaten += 1;
        if(numberOfEnemies == numberOfRoundEnemiesBeaten)
            Invoke("PlayerWon", 2);

    }

    private void PlayerWon()
    {
        //player won the round
        roundInProgress = false;
        roundStartScreen.SetActive(false);
        roundsWon += 1;

        foreach (GameObject enemy in enemies.Values)
            Destroy(enemy);
        enemies.Clear();
        numberOfAllEnemiesBeaten += numberOfRoundEnemiesBeaten;
        numberOfRoundEnemiesBeaten = 0;

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

    public void gameOver()
    {

        player.GetComponent<character_controller>().enabled = false;
        camera.GetComponent<mouse_aim>().enabled = false;

        //player lost the round
        roundInProgress = false;
        foreach (GameObject enemy in enemies.Values)
            Destroy(enemy);
        enemies.Clear();
        numberOfRoundEnemiesBeaten = 0;

        roundStartScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 150f;

        gameOverScreen.transform.Find("Round stats").Find("Enemies Beaten").Find("Number").GetComponent<Text>().text = numberOfAllEnemiesBeaten.ToString();
        gameOverScreen.transform.Find("Round stats").Find("Rounds Won").Find("Number").GetComponent<Text>().text = roundsWon.ToString();

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

        previousEnemiesBeaten = numberOfAllEnemiesBeaten;
    }

    public void restartLastRound()
    {
        player.GetComponent<character_actions_manager>().isBeingHit = false;
        if (player.GetComponent<character_actions_manager>().foam.GetComponent<ParticleSystem>().isPlaying)
            player.GetComponent<character_actions_manager>().foam.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        player.GetComponent<character_stats>().health = 0f;
        player.GetComponent<character_actions_manager>().hasLost = false;
        player.GetComponent<Animator>().SetBool("is_beaten", false);
        player.GetComponent<Animator>().Play("Idle", 0);

        numberOfAllEnemiesBeaten = previousEnemiesBeaten;
        roundInProgress = false;

        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 0f;

        player.GetComponent<character_controller>().enabled = true;
        camera.GetComponent<mouse_aim>().enabled = true;

        gameOverIsOpen = false;

        player.transform.position = new Vector3(0, 0, -100);
    }
}
