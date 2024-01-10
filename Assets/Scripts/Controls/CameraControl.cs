using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera cam;
    [SerializeField]
    GameObject cameraRig;
    [SerializeField]
    GameObject playermodel;

    [SerializeField]
    private float sensitivity = 0.006f;

    PlayerInput input;
    InputAction rotateCameraAction;
    InputAction zoomCameraAction;
    GameObject player;
    int mousePosX;
    //int FOV = 40;
    bool isHeld = false;

    Vector2 p1;
    Vector2 p2;
    CameraControl instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        DontDestroyOnLoad(this.gameObject.transform.parent);
        }
        else
        Destroy(this.gameObject.transform.parent);
      //  input = GetComponent<PlayerInput>();
//        rotateCameraAction = input.actions["Camera"];
//        zoomCameraAction = input.actions["Zoom"];
//        cam.m_Lens.FieldOfView = FOV;
//        player = GameObject.FindGameObjectWithTag("MainPlayer");
//        cam.Follow = player.transform;
//        cam.LookAt = player.transform;
    }


    private void Start()
    {
      // CinemachineCore.GetInputAxis = CustomAxis;
        playermodel = GameManager.Instance._player;
    }

    // float CustomAxis(string axis)
    // {
    //     if (axis == "Horizontal")
    //         if (isHeld)
    //             return UnityEngine.Input.GetAxis(axis);
    //         else
    //             return 0;

    //     if (axis == "Vertical")
    //         if (isHeld)
    //             return UnityEngine.Input.GetAxis(axis);
    //         else
    //             return 0;

    //     return UnityEngine.Input.GetAxis(axis);
    // }

    private void Update()
    {
        cameraRig.transform.position = playermodel.transform.position;
        // rotateCameraAction.started += _ => { mousePosX = Mathf.RoundToInt(Input.mousePosition.x);  isHeld = true; };
        // rotateCameraAction.canceled += _ => { isHeld = false; };
        
        // zoomCameraAction.performed += _ => {
        //     if (zoomCameraAction.ReadValue<float>() > 0)
        //         if (cam.m_Lens.FieldOfView >= 25)
        //             cam.m_Lens.FieldOfView -= 5;

        //     if (zoomCameraAction.ReadValue<float>() < 0)
        //         if(cam.m_Lens.FieldOfView <= 55)
        //             cam.m_Lens.FieldOfView += 5;
        // };

        // if (!isHeld)
        // {
        //    cam.m_XAxis.m_MaxSpeed = 0f;

        // }

        // if (isHeld)
        // {
        // cam.m_XAxis.m_MaxSpeed = 15f;
        // cam.m_XAxis.Value = Mathf.RoundToInt((Input.mousePosition.x - mousePosX) * sensitivity);
        //    float dx = (Input.mousePosition.x - mousePosX) * sensitivity;
        //    cameraRig.transform.rotation*= Quaternion.Euler(new Vector3(0,dx,0));
        //    mousePosX = Input.mousePosition.x;
        // }
        getCameraRotation();
    }

    void getCameraRotation()
    {
        if(Input.GetMouseButtonDown(2))
        {
            p1=Input.mousePosition;
        }

        if(Input.GetMouseButton(2))
        {
           p2 = Input.mousePosition;
           float dx = (p2-p1).x * sensitivity;
           cameraRig.transform.rotation*= Quaternion.Euler(new Vector3(0,dx,0));
           p1 = p2;
        }


    }
}
