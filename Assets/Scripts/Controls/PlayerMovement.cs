using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed, rotation_speed;
    private PlayerInput input;
    private InputAction followAction;
    private InputAction walkAction;
    private InputAction runAction;
    private InputAction keyAction;
    private Ray ray;
    private RaycastHit hit;

    private Vector3 cameraForward;

    [SerializeField]
    NavMeshAgent navPlayer;

    private void Awake()
    {
        navPlayer = null;
        navPlayer = GetComponent<NavMeshAgent>();

        input = GetComponent<PlayerInput>();
        followAction = input.actions["Follow"];
        walkAction = input.actions["Walk"];
        runAction = input.actions["Run"];
        keyAction = input.actions["Keys"];
    }

    private void Update()
    {
        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward = Vector3.Normalize(cameraForward);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 150f, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward * 150f, Color.blue);


        walkAction.performed += _ => { MoveMouse(3.5f); };
            runAction.performed += _ => MoveMouse(10f);
            
            if(followAction.phase == InputActionPhase.Performed) { MoveMouse(5f);  }
            if(followAction.phase == InputActionPhase.Canceled) { Stop();  }

            if(keyAction.phase == InputActionPhase.Started) MoveKeybard();
    }
    private void MoveMouse(float speed)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            navPlayer.speed = speed;
            navPlayer.destination = hit.point;
        }
    }

    private void MoveKeybard()
    {
        var i = 0;
        while (i < 10)
        {
            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                Stop();
                transform.position += Camera.main.transform.forward * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            }
            if (Input.GetKey("down") || Input.GetKey("s"))
            {
                Stop();
                transform.position += -Camera.main.transform.forward * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.LookRotation(-cameraForward, Vector3.up);
            }
            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                Stop();
                transform.position += -Camera.main.transform.right * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.LookRotation(-Camera.main.transform.right, Vector3.up);
            }
            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                Stop();
                transform.position += Camera.main.transform.right * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.LookRotation(Camera.main.transform.right, Vector3.up);
            }
            i++;
        }
    }

    private void Stop()
    {
        navPlayer.ResetPath();
    }
}
