using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.Rotate(30f, 180f, 0);
    }

    private void Update()
    {
        Vector3 v = cam.transform.position - transform.position;
        v.x = v.z = 0f;
        //transform.LookAt(cam.transform.position - v);
        //transform.LookAt(Camera.main.transform);
    }
}
