﻿using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

    public Camera my_camera;
    Quaternion direction = new Quaternion();

    // Use this for initialization
    void Start()
    {
        my_camera = Camera.main;

        direction.x = transform.localRotation.x;
        direction.y = transform.localRotation.y;
        direction.z = transform.localRotation.z;
        direction.w = transform.localRotation.w;
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = null;
        if (camera != null)
        {
            cam = my_camera;
        }
        else
        {
            cam = Camera.current;
            if (!cam)
                return;
        }
        transform.rotation = cam.transform.rotation * direction;
    }

}
