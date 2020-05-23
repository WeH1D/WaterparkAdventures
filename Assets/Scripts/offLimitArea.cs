using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offLimitArea : MonoBehaviour
{
    public bool instantLoss;

    public GameObject gameLogic;
    public GameObject player;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("player") && instantLoss)
            gameLogic.GetComponent<gameLogic>().gameOver();

        if (collider.CompareTag("player") && !instantLoss)
        {
            collider.gameObject.GetComponent<character_actions_manager>().damageRecieved = 0.2f;
            collider.gameObject.GetComponent<character_actions_manager>().isBeingHit = true;

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("player") && !instantLoss)
            collider.gameObject.GetComponent<character_actions_manager>().isBeingHit = false;
    }
}
