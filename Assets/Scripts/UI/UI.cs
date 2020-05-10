using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public character_stats player;

    private GameObject AmmoBar;
    private GameObject Money;
    private GameObject Health;
    private GameObject ReloadIcon;
    private GameObject ShieldIcon;

    // Start is called before the first frame update
    void Start()
    {
        AmmoBar = transform.Find("Ammo Bar").gameObject;
        ReloadIcon = AmmoBar.transform.Find("Reload").gameObject;
        Money = transform.Find("Money").gameObject;
        Health = transform.Find("Health").gameObject;
        ShieldIcon = Health.transform.Find("Shield").gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        Health.transform.Find("Slider").GetComponent<Slider>().value = player.health;
        Health.transform.Find("Slider").GetComponent<Slider>().maxValue = 1;
        Health.transform.Find("Slider").GetComponent<Slider>().minValue = 0;
        Health.transform.Find("HealthValue").GetComponent<Text>().text = ((int)player.health).ToString();

        Money.transform.Find("Text").GetComponent<Text>().text = player.money.ToString();

        if (player.equiped_weapon)
        {
            AmmoBar.transform.Find("Slider").GetComponent<Slider>().value = player.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo;
            AmmoBar.transform.Find("Slider").GetComponent<Slider>().maxValue = player.maxAmmo;
            AmmoBar.transform.Find("AmmoValue").GetComponent<Text>().text = ((int)player.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo).ToString();
        }
        else
        {
            AmmoBar.transform.Find("Slider").GetComponent<Slider>().value = 0;
            AmmoBar.transform.Find("AmmoValue").GetComponent<Text>().text = "0";
        }
        if (player.isReloading)
            ReloadIcon.SetActive(true);
        else
            ReloadIcon.SetActive(false);

        if (player.isShielded)
            ShieldIcon.SetActive(true);
        else
            ShieldIcon.SetActive(false);
    }
}
