using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }
}
