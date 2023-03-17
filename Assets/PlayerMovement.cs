using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private PlayerInput input;
    private InputAction followAction;
    private InputAction walkAction;
    private InputAction runAction;
    private InputAction keyAction;

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
    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouse++;
            Debug.Log(mouse);
            Move();
        }
        if(Input.GetMouseButtonUp(0))
        {

        }
    }*/

    private void Update()
    {
            Vector3 key = keyAction.ReadValue<Vector3>();
            walkAction.performed += _ => Move(3.5f);
            runAction.performed += _ => Move(10f);
            
            if(followAction.phase == InputActionPhase.Performed) { Move(5f);  }
            if(followAction.phase == InputActionPhase.Canceled) { Stop();  }

            if(keyAction.phase == InputActionPhase.Performed)
            {
            Debug.Log("Hemlo");
            Vector3 test = new Vector3(0, 25, 0);
            if (key.x != 0)
                transform.Rotate(test);
                if (key.z != 0)
                {
                    Debug.Log("¯yjê");
                    //transform.position += transform.forward * key.z * Time.deltaTime * navPlayer.speed;
                }
            }
    }

    private void Move(float speed)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
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
