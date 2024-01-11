using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDead : MonoBehaviour
{
    [SerializeField]
    List<string> playDeadAfterEncounter;
    void Start()
    {
        foreach(string s in playDeadAfterEncounter)
            if (EncounterList.Instance.GetEncounter(s))
            {
                NPCAnimationCenter npc = gameObject.GetComponent<NPCAnimationCenter>();
                npc.ResetAnimations();
                npc.SetAnimation("isDead");
            }
    }
}
