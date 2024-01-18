using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCAnimationCenter : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NPCAnimations activeAnimation;

    [SerializeField]
    Dictionary<string, bool> npcAnimation = new Dictionary<string, bool>()
    {
        {"isBartering", false},
        {"isSitting", false},
        {"isDrinking", false},
        {"isYelling", false},
        {"isArguing", false},
        {"isFishing", false},
        {"isLeaning", false},
        {"isDead", false},
    };

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(activeAnimation.ToString(), true);
    }

    public void ResetAnimations()
    {
        foreach (string s in npcAnimation.Keys.ToList())
            npcAnimation[s] = false;
        animator.SetBool(activeAnimation.ToString(), false);
    }

    public void SetAnimation(string s)
    {
        npcAnimation[s] = true;
        animator.SetBool(s, true);
        activeAnimation = (NPCAnimations)System.Enum.Parse(typeof(NPCAnimations), s);
    }
}

public enum NPCAnimations
{
    isBartering,
    isSittingHands,
    isDrinking,
    isYelling,
    isFishing,
    isLeaning,
    isDead
}