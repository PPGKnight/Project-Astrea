using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationAfterEncounter : MonoBehaviour
{
    [SerializeField]
    List<string> playAfterEncounter;

    [SerializeField] NPCAnimations animationAfterEncounter;
    [SerializeField] bool moveNPC;
    [SerializeField] Vector3 cordsPos, cordsRot;

    void Start()
    {
        foreach(string s in playAfterEncounter)
            if (EncounterList.Instance.GetEncounter(s))
            {
                NPCAnimationCenter npc = gameObject.GetComponent<NPCAnimationCenter>();
                npc.ResetAnimations();
                npc.SetAnimation(animationAfterEncounter.ToString());

                if (moveNPC)
                {
                    transform.localPosition = cordsPos;
                    transform.localRotation = Quaternion.Euler(cordsRot);
                }
            }
    }
}
