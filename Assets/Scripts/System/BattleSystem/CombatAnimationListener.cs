using System;
using UnityEngine;

public class CombatAnimationListener : MonoBehaviour
{
    public static event Action AnimationFinished;

    public void AnimationDone()
    {
        Debug.Log("Animacja zakonczona!");
        AnimationFinished?.Invoke();
    }
}
