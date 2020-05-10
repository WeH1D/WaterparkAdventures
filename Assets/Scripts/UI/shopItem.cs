using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopItem : MonoBehaviour
{
    public GameObject item;
    private GameObject price;
    

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        price = transform.Find("Price").gameObject;

        price.GetComponent<Text>().text = item.GetComponent<equippable>().price.ToString();
    }

    public void OnItemClicked()
    {
        if(player.GetComponent<character_stats>().money >= item.GetComponent<equippable>().price)
        {
            Instantiate(item, player.transform.position, new Quaternion());
            player.GetComponent<character_stats>().money -= item.GetComponent<equippable>().price;
        }
    }
}
