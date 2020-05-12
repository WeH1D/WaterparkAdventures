using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startButton : MonoBehaviour
{
    public character_controller controller;

    public bool startRound;
    public bool inRange;

    void Start()
    {
        startRound = false;
        inRange = false;
    }

    void Update()
    {
        if(inRange)
        {
            if (Input.GetKeyDown(controller.Interact))
                startRound = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("player"))
            inRange = true;
    }

    void onTriggerExit(Collider collider)
    {
        if (collider.CompareTag("player"))
            inRange = false;
    }
}
