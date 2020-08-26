using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    public bool enable;

    void Start()
    {
        enable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            enable = !enable;
            cam1.enabled = enable;
            cam2.enabled = !enable;
        }
    }
}
