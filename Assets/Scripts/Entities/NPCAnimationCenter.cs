using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationCenter : MonoBehaviour
{
     [SerializeField] Animator animator;
     [SerializeField] int isBartering;
     [SerializeField] int isSitting;
      [SerializeField] int isDrinking;
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

    }
}