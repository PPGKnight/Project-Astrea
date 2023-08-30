using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    public float movement_speed, rotation_speed;
    private PlayerInput input;
    private InputAction followAction;
    private InputAction walkAction;
    private InputAction runAction;
    private InputAction keyAction;
    private InputAction interactAction;
    private Ray ray;
    private RaycastHit hit;

    private Vector3 cameraForward;
    public static event Action InteractionWithNPC;

    [SerializeField]
    NavMeshAgent navPlayer;

    static PlayerMovement _instance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navPlayer = null;
        navPlayer = GetComponent<NavMeshAgent>();

        input = GetComponent<PlayerInput>();
        followAction = input.actions["Follow"];
        walkAction = input.actions["Walk"];
        runAction = input.actions["Run"];
        keyAction = input.actions["Keys"];
        interactAction = input.actions["Interact"];

        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        MovementAnimation();
        
        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward = Vector3.Normalize(cameraForward);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 150f, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward * 150f, Color.blue);

        interactAction.performed += _ => { InteractionWithNPC?.Invoke(); };

        walkAction.performed += _ => { MoveMouse(3.5f); };
            runAction.performed += _ => MoveMouse(10f);
            
            if(followAction.phase == InputActionPhase.Performed) { MoveMouse(5f);  }
            if(followAction.phase == InputActionPhase.Canceled) { StopAnim();  }

            if(keyAction.phase == InputActionPhase.Started) MoveKeybard();
            
    }
    void MovementAnimation()
    {
        if (navPlayer.velocity.magnitude == 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if (navPlayer.velocity.magnitude > 0 && navPlayer.velocity.magnitude < 4f)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
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
                animator.SetBool("isWalking", true);
                Stop();
                transform.position += Camera.main.transform.forward * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), Time.deltaTime);
            }
            
            if (Input.GetKey("down") || Input.GetKey("s"))
            {
               animator.SetBool("isWalking",true);
                Stop();
                transform.position += -Camera.main.transform.forward * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-cameraForward, Vector3.up), Time.deltaTime);
            }
            
            if (Input.GetKey("left") || Input.GetKey("a"))
            {
               animator.SetBool("isWalking",true);
                Stop();
                transform.position += -Camera.main.transform.right * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-Camera.main.transform.right, Vector3.up), Time.deltaTime);
            }
           
            if (Input.GetKey("right") || Input.GetKey("d"))
            {
               animator.SetBool("isWalking",true);
                Stop();
                transform.position += Camera.main.transform.right * Time.deltaTime * movement_speed * GameManager.Instance.worldTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.right, Vector3.up), Time.deltaTime);
            }
            i++;
        }
      
            
    }

    public void UpdateAgent(Vector3 newPos)
    {
        Stop();
        navPlayer.Warp(newPos);
    }

    private void Stop()
    {
        navPlayer.ResetPath();
    }
    private void StopAnim()
    {
        navPlayer.ResetPath();
        animator.SetBool("isWalking", false);
    }

}
