using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
public class DialogueList
{
    static Dictionary<string, bool> DIALOGUE_LIST = new Dictionary<string, bool>{
        {"TavernTutorialPreFight", false },
        {"TavernTutorialPostFight", false },
        {"TavernTourWithGary", false},
        {"001_GatherForSmith", false},
        {"001_OneTimePickaxe", false},
        {"002_CheckLumbererForTheSmith", false},
        {"002_ArrivingAtTheLumber", false},
        {"002_AfterTheFight", false}
    };

    public void UpdateDialogue(string key)
    {
        DIALOGUE_LIST[key] = true;
        DialogueManager.Instance.CheckRequirements();
    }

    public bool CheckIfCompleted(string key)
    {
        return DIALOGUE_LIST[key];
    }
}
