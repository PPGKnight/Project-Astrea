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


    NavMeshAgent navPlayer;
    Vector3 transPlayer;

    private void Awake()
    {
        navPlayer = GetComponent<NavMeshAgent>();
        transPlayer = GetComponent<Transform>().position;

        input = GetComponent<PlayerInput>();
        followAction = input.actions["Follow"];
        walkAction = input.actions["Walk"];
        runAction = input.actions["Run"];
        keyAction = input.actions["Keys"];
        
    }

    private void Update()
    {
            walkAction.performed += _ => Move(3.5f);
            runAction.performed += _ => Move(10f);
            
            if(followAction.phase == InputActionPhase.Performed) { Move(5f);  }
            if(followAction.phase == InputActionPhase.Canceled) { Stop();  }

            var i = 0;
            while(i < 10){
                if(Input.GetKey("up") || Input.GetKey("w")){
                    transform.position += transform.forward * Time.deltaTime * movement_speed;
                }
                if(Input.GetKey("down") || Input.GetKey("s")){
                    transform.position += -transform.forward * Time.deltaTime * movement_speed;
                }
                if(Input.GetKey("left") || Input.GetKey("a")){
                    transform.Rotate(0f, -rotation_speed * Time.deltaTime, 0f);
                }
                if(Input.GetKey("right") || Input.GetKey("d")){
                    transform.Rotate(0f, rotation_speed * Time.deltaTime, 0f);
                }
            i++;
            }
    }
    private void Move(float speed)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            navPlayer.speed = speed;
            navPlayer.destination = hit.point;
        }

    }
    private void Stop()
    {
        navPlayer.speed = 0f;
    }
}
