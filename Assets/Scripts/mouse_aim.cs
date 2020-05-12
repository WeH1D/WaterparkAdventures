using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_aim : MonoBehaviour
{
    public character_controller controller;
    public GameObject player;
    public float rotate_speed;
    Vector3 offsetNoAim;
    Vector3 offsetAim;
    bool isAming;

    bool isHittingWall;
    public GameObject lookAtWhenHittingWall;
    Vector3 wouldBeCamPos;

#pragma warning disable 0649
    [Header("Camera Position Managment")]
    [SerializeField]
    private GameObject lookAtNoAim;
    [SerializeField]
    private GameObject lookAtAim;
    [SerializeField]
    private GameObject cameraDefaultPos;
    [SerializeField]
    private GameObject cameraAimPos;
    GameObject lookAt;
    private Vector3 camPos;
   #pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        lookAt = lookAtNoAim;
        offsetNoAim = lookAtNoAim.transform.position - cameraDefaultPos.transform.position;
        offsetAim = lookAtAim.transform.position - cameraAimPos.transform.position;
        isHittingWall = false;
        wouldBeCamPos = cameraDefaultPos.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotate_speed;
        float vertical = Input.GetAxis("Mouse Y") * rotate_speed;
        if (Input.GetKey(controller.Aim))
            isAming = true;
        else
            isAming = false;

        if (isAming)
        {
            //player is aiming, camera sets to according position
            lookAt = lookAtAim;
            player.transform.Rotate(0, horizontal, 0);
        }
        else
        {
            //player is not aiming, camera sets at default position and moves freely around the player
            lookAt = lookAtNoAim;
            //lookAt.transform.Rotate(0, horizontal, 0);
            player.transform.Rotate(0, horizontal, 0);

        }


        int layerMask = 1 << 10;

        //Detects if the camera is abput to get behind the border
        RaycastHit hitInfo;
        if (Physics.Linecast(new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z), wouldBeCamPos, out hitInfo, layerMask))
        {
            Debug.DrawLine(new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z), wouldBeCamPos, Color.red);
            isHittingWall = true;
            
        }
        else
        {
            isHittingWall = false;
            wouldBeCamPos = transform.position;
        }

        float desired_angle = lookAt.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desired_angle, 0);

        if (isHittingWall)
        {
            //Camera isn't allowed behind the wall border, but the raycast is still cast towards the "would be position" of the camera so 
            //it knows when the camera can be back at its original position
            if(isAming)
                wouldBeCamPos = lookAt.transform.position - (rotation * offsetAim);
            else
                wouldBeCamPos = lookAt.transform.position - (rotation * offsetNoAim);

            transform.position = new Vector3(hitInfo.point.x, wouldBeCamPos.y, hitInfo.point.z);
        }
        else
        {
            if (isAming)
                transform.position = lookAt.transform.position - (rotation * offsetAim);
            else
                transform.position = lookAt.transform.position - (rotation * offsetNoAim);
        }

        if (isHittingWall)
        {
            if(isAming)
                transform.LookAt(lookAt.transform);
            else
                transform.LookAt(lookAtWhenHittingWall.transform);
        }
        else
            transform.LookAt(lookAt.transform);
    
        //when character is standing and wants to move, it will move in the direction the camera is facing -> this wont hapen if character is in aim mode
        //if (!isAming && (player.GetComponent<Animator>().GetBool("is_walking") || player.GetComponent<Animator>().GetBool("is_running")))
        //{
        //    Quaternion orginalRot = lookAt.transform.rotation;
        //    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, lookAt.transform.rotation, Time.deltaTime * 10);
        //    lookAt.transform.rotation = orginalRot;
        //}

    }
}
