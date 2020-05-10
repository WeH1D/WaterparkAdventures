using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_controller : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Character Controls")]
    public KeyCode Jump;
    public KeyCode Run;
    public KeyCode Attack;
    public KeyCode Aim;
    public KeyCode Inventory;
    public KeyCode Drop;
    public KeyCode Interact;
    public KeyCode PauseScreen;

    [Header("Character components")]
    [SerializeField]
    private Animator player_animator;
    [SerializeField]
    character_stats characterStats;

#pragma warning restore 0649

    [Header("Current character speed")]
    private float Speed;
    private float JumpSpeed;
    private Vector3 Gravity = new Vector3();

    private bool isGrounded;

    void Start()
    {
        Speed = characterStats.walking_speed;
        JumpSpeed = characterStats.jump_speed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(hor, 0 ,ver);

        #region Player Movement
        if (ver != 0 || hor != 0)
        {
            //while aiming the player cant run at the same time
            if (Input.GetKey(Run) && !Input.GetKey(Aim))
            {
                Speed = characterStats.run_speed;
                player_animator.SetBool("is_running", true);
                player_animator.SetBool("is_walking", false);

            }
            else
            {
                Speed = characterStats.walking_speed;
                player_animator.SetBool("is_running", false);
                player_animator.SetBool("is_walking", true);
            }

        }
        else
        {
            player_animator.SetBool("is_walking", false);
            player_animator.SetBool("is_running", false);
        }

        if (Input.GetKey(Aim))
            Speed -= 15;

        playerMovement *= Speed;

        if (transform.GetComponent<CharacterController>().isGrounded)
        {
            Gravity = Vector3.zero;

            if (Input.GetKeyDown(Jump))
            {
                Gravity.y = JumpSpeed;
                player_animator.SetBool("is_jumping", true);
                isGrounded = false;
            }
            else
            {
                player_animator.SetBool("is_jumping", false);
                isGrounded = true;
            }
        }
        else
            Gravity += Physics.gravity * Time.deltaTime;

        playerMovement += Gravity;
        playerMovement = transform.TransformDirection(playerMovement);

        transform.GetComponent<CharacterController>().Move(playerMovement * Time.deltaTime);

        #endregion

        #region Attacking
        if (Input.GetKey(Attack) && isGrounded)
        {
            if (characterStats.equiped_weapon)
            {
                if (characterStats.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo <= 0)
                {
                    player_animator.SetBool("attack_one_hand", false);
                    characterStats.equiped_weapon_object.GetComponent<weapon>().stopShooting();
                }
                else
                {
                    player_animator.SetBool("attack_one_hand", true);
                    characterStats.equiped_weapon_object.GetComponent<weapon>().CurrentAmmo -= characterStats.equiped_weapon_object.GetComponent<weapon>().rateOfAmmoSpending;
                    characterStats.equiped_weapon_object.GetComponent<weapon>().startShooting();
                }
            }
        }
        else
        {
            player_animator.SetBool("attack_one_hand", false);
            if (characterStats.equiped_weapon)
                characterStats.equiped_weapon_object.GetComponent<weapon>().stopShooting();
        }

        #endregion

    }
}
