using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startButton : MonoBehaviour
{
    public character_controller controller;

    public bool startRound; 

    void Start()
    {
        startRound = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("player"))
        {
            if (Input.GetKeyDown(controller.Interact))
                startRound = true;

        }
    }
}
