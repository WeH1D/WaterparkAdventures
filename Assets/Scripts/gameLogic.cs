using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class gameLogic : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject startButton;
    public GameObject winScreen;

    GameObject player;
    GameObject camera;

    public GameObject[] spawnPoints;
    public GameObject spawnedEnemies;

    Dictionary<int, GameObject> enemies;

    public int numberOfEnemies = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        enemies = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //player hit the button and started the round
        if (startButton.GetComponent<startButton>().startRound)
            spawnEnemies();

    }

    void spawnEnemies()
    {
        startButton.GetComponent<startButton>().startRound = false;
        for (int i = 0; i < numberOfEnemies; i++)
            enemies.Add(i, Instantiate(enemyPrefab, spawnPoints[i].transform.position, new Quaternion(), spawnedEnemies.transform));
    }

    public IEnumerator despawnEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(3);
        Destroy(enemy);
        checkIfPlayerWon();
    }

    private void checkIfPlayerWon()
    {
        //player won the round
        if(spawnedEnemies.transform.childCount == 0)
        {
            winScreen.SetActive(true);
            player.GetComponent<character_controller>().enabled = false;
            camera.GetComponent<mouse_aim>().enabled = false;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            camera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength.value = 150f;
        }
    }
}
