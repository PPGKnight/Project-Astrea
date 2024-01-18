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
    [SerializeField]
    private float zoomsensitivity = 0.006f;


    PlayerInput input;
    InputAction rotateCameraAction;
    InputAction zoomCameraAction;
    GameObject player;
    int mousePosX;
    int FOV = 40;
    bool isHeld = false;

    Vector2 p1;
    Vector2 p2;
    CameraControl instance;
    float minHeight = 0f;
    float maxHeight = 2f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        DontDestroyOnLoad(this.gameObject.transform.parent);
        }
        else
        Destroy(this.gameObject.transform.parent);
        input = GetComponent<PlayerInput>();
//        rotateCameraAction = input.actions["Camera"];
        zoomCameraAction = input.actions["Zoom"];
        cam.m_Lens.FieldOfView = FOV;
//        player = GameObject.FindGameObjectWithTag("MainPlayer");
//        cam.Follow = player.transform;
//        cam.LookAt = player.transform;
    }


    private void Start()
    {
      // CinemachineCore.GetInputAxis = CustomAxis;
        //playermodel = GameManager.Instance._player;
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
        
        zoomCameraAction.performed += _ => {

        float scrollSp = -zoomsensitivity * zoomCameraAction.ReadValue<float>();

                 if (zoomCameraAction.ReadValue<float>() > 0)
                     if (cam.m_Lens.FieldOfView >= 40)
                        cam.m_Lens.FieldOfView -= 5;

                 if (zoomCameraAction.ReadValue<float>() < 0)
                    if(cam.m_Lens.FieldOfView <= 50)
                        cam.m_Lens.FieldOfView += 5;

            if (zoomCameraAction.ReadValue<float>() > 0)
        {
            Debug.Log("bitchin"+Mathf.Log(transform.position.y));
            Debug.Log(zoomCameraAction.ReadValue<float>());
        }

        if((transform.position.y >= maxHeight) && (scrollSp > 0))
        {
            scrollSp = 0;
        }
        else if((transform.position.y <= minHeight) && (scrollSp < 0))
        {
           scrollSp = 0; 
        }

        if((transform.position.y + scrollSp) > maxHeight)
        {
            scrollSp = transform.position.y - maxHeight;
        }
        else if((transform.position.y + scrollSp) < minHeight)
        {
            scrollSp = minHeight -transform.position.y;
        }

        Vector3 cameraHeight = new Vector3(0,scrollSp,0);

        transform.position = transform.position + cameraHeight;
        };
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
