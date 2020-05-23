using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemy_AI : MonoBehaviour
{
    [SerializeField]
    GameObject destination;

    NavMeshAgent navMeshAgent;

    GameObject gameLogic;

    public float health;
    public GameObject healthbar;

    private Animator anim;
    private enemy_stats enemy_stats;
    public GameObject weapon;
    private float damageRecived;
    private ParticleSystem water_stream;

    private GameObject player;
    public float[] min_max_distance_from_player;

    bool isBeingHit = false;
    bool isFainted = false;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("gameLogic");

        player = GameObject.FindGameObjectWithTag("player");
        healthbar.GetComponent<Slider>().value = health;
        anim = transform.GetComponent<Animator>();
        enemy_stats = transform.GetComponent<enemy_stats>();
        water_stream = weapon.transform.Find("water_stream").Find("water").GetComponent<ParticleSystem>();

        StartCoroutine(attackLogic());
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("player");

    }

    void Update()
    {
        if (isBeingHit)
        {
            health += damageRecived * Time.deltaTime;
            healthbar.GetComponent<Slider>().value = health;
        }

        if (health >= 1f && !isFainted)
        {
            isFainted = true;
            faint();
            gameLogic.GetComponent<gameLogic>().despawnEnemy();
        }
        else if(!isFainted)
        {
            navMeshAgent.SetDestination(destination.transform.position);

            float distance_from_player = Vector3.Distance(transform.position, player.transform.position);

            setAllParamsFalse();

            if (distance_from_player <= min_max_distance_from_player[1])
            {
                transform.LookAt(new Vector3(destination.transform.position.x, transform.position.y, destination.transform.position.z));
                if (distance_from_player > min_max_distance_from_player[0])
                    walk();
                else
                    stand();
            }
            else
                run();
        }

    }

    void stand()
    {
        setAllParamsFalse();
    }
    void walk()
    {
        anim.SetBool("is_walking", true);
        //navMeshAgent.speed = 10;


    }
    void run()
    {
        anim.SetBool("is_running", true);
        //navMeshAgent.speed = 30;


    }
    void faint()
    {
        setAllParamsFalse();
        anim.SetBool("is_beaten", true);

        if (water_stream.isPlaying)
            water_stream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        navMeshAgent.SetDestination(transform.position);
    }
    void setAllParamsFalse()
    {
        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if (parameter.name != "attack_one_hand")
                anim.SetBool(parameter.name, false);
        }
    }



    IEnumerator attackLogic()
    {
        bool attack = false;

        while (true)
        {
            float minWait = 2f;
            float maxWait = 6f;

            yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
            if (!attack && !anim.GetBool("is_running"))
            {
                Attack();
                attack = true;
            }
            else
            {
                DontAttack();
                attack = false;
            }
            
        }
    }
    private void Attack()
    {
        anim.SetBool("attack_one_hand", true);

        if (!water_stream.isPlaying)
            water_stream.Play();
    }
    private void DontAttack()
    {
        anim.SetBool("attack_one_hand", false);
        if (water_stream.isPlaying)
            water_stream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }



    void OnTriggerStay(Collider colision)
    {
        if (colision.CompareTag("player_weapon") && player.GetComponent<Animator>().GetBool("attack_one_hand"))
        {
            isBeingHit = true;
            damageRecived = colision.transform.parent.GetComponent<weapon>().damage;

            GameObject foam = transform.Find("foam").gameObject;

            if (!foam.GetComponent<ParticleSystem>().isPlaying)
                foam.GetComponent<ParticleSystem>().Play();
        }

    }
    void OnTriggerExit(Collider colision)
    {
        if (colision.CompareTag("player_weapon"))
        {
            isBeingHit = false;

            GameObject foam = transform.Find("foam").gameObject;
            if (foam.GetComponent<ParticleSystem>().isPlaying)
                foam.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
