using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationCenter : MonoBehaviour
{
     [SerializeField] Animator animator;
     [SerializeField] int isBartering;
     [SerializeField] int isSitting;
     [SerializeField] int isDrinking;
     [SerializeField] int isYelling;
     [SerializeField] int isArguing;
     [SerializeField] int isFishing;
     [SerializeField] int isLeaning;
     [SerializeField] int isDead;

    // Start is called before the first frame update
    void Start()
    {
         animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBartering == 1)
        {
            animator.SetBool("isBartering",true);
        }
        if(isSitting == 1)
        {
            animator.SetBool("isSittingHands",true);
        }
        if(isDrinking == 1)
        {
            animator.SetBool("isDrinking",true);
        }
        if(isYelling == 1)
        {
            animator.SetBool("isYelling",true);
        }
        if(isArguing == 1)
        {
            animator.SetBool("isArguing",true);
        }
        if(isFishing == 1)
        {
            animator.SetBool("isFishing",true);
        }
         if(isLeaning == 1)
        {
            animator.SetBool("isLeaning",true);
        }
        if(isDead == 1)
        {
            animator.SetBool("isDead",true);
        }

    }

    public void ResetAnimations()
    {
        animator.SetBool("isBartering", false);
        animator.SetBool("isSittingHands", false);
        animator.SetBool("isDrinking", false);
        animator.SetBool("isYelling", false);
        animator.SetBool("isArguing", false);
        animator.SetBool("isFishing", false);
        animator.SetBool("isLeaning", false);
        animator.SetBool("isDead", false);
    }

    public void SetAnimation(string s)
    {
        animator.SetBool(s, true);
    }
}
