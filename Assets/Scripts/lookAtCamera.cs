using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    private new GameObject camera;

    // Update is called once per frame\

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}
