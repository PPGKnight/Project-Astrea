using System;
using UnityEngine;

public class CombatMouseListener : MonoBehaviour
{
    public static event Action MouseClicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MouseClicked?.Invoke();
    }
}
