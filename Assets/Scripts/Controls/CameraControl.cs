using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook cam;
    [SerializeField]
    private float sensitivity = 0.006f;

    private PlayerInput input;
    private InputAction rotateCameraAction;
    private InputAction zoomCameraAction;
    private GameObject player;
    private int mousePosX;
    private int FOV = 40;
    bool isHeld = false;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rotateCameraAction = input.actions["Camera"];
        zoomCameraAction = input.actions["Zoom"];
        cam.m_Lens.FieldOfView = FOV;
        player = GameObject.FindGameObjectWithTag("MainPlayer");
        cam.Follow = player.transform;
        cam.LookAt = player.transform;
    }

    private void Update()
    {
        rotateCameraAction.started += _ => { mousePosX = Mathf.RoundToInt(Input.mousePosition.x);  isHeld = true; };
        rotateCameraAction.canceled += _ => { isHeld = false; };

        zoomCameraAction.performed += _ => {
            if (zoomCameraAction.ReadValue<float>() > 0)
                if (cam.m_Lens.FieldOfView <= 55)
                    cam.m_Lens.FieldOfView += 5;

            if (zoomCameraAction.ReadValue<float>() < 0)
                if(cam.m_Lens.FieldOfView >= 25)
                    cam.m_Lens.FieldOfView -= 5;
        };
        
        if (isHeld)
            cam.m_XAxis.Value = Mathf.RoundToInt((Input.mousePosition.x - mousePosX) * sensitivity);
    }
}
