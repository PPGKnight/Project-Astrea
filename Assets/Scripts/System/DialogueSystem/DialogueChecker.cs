using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChecker : MonoBehaviour
{
    [SerializeField] GameObject dialogueTrigger;
    [SerializeField] List<string> dialogueRequirements;
    [SerializeField] List<string> encounterRequirements;
    [SerializeField] List<QuestSO> questRequirements;


    bool canBeActive = true;
    private void Start()
    {
        CheckAll();
    }

    private void OnEnable()
    {
        DialogueManager.CheckDialogues += CheckAll; 
    }

    private void OnDisable()
    {
        DialogueManager.CheckDialogues -= CheckAll; 
    }

    public void CheckAll()
    {
        canBeActive = true;
        CheckDialogue();
        CheckEncounter();
        CheckQuest();
        dialogueTrigger.SetActive(canBeActive);
    }

    void CheckDialogue()
    {
        if (DialogueManager.Instance.dialogueList.CheckIfCompleted(this.gameObject.name))
        {
            canBeActive = false;
            return;
        }

        if (dialogueRequirements.Count > 0)
        {
            foreach (var r in dialogueRequirements)
            {
                if (!DialogueManager.Instance.dialogueList.CheckIfCompleted(r))
                    canBeActive = false;
            }
        }
        else
        {
            if (canBeActive == false) return;
            canBeActive = true;
        }
    }

    void CheckEncounter()
    {
        if (encounterRequirements.Count > 0)
        {
            foreach (var r in encounterRequirements)
            {
                if (!EncounterList.Instance.GetEncounter(r))
                    canBeActive = false;
            }
        }
        else
        {
            if (canBeActive == false) return;
            canBeActive = true;
        }
    }

    void CheckQuest()
    {
        if (questRequirements.Count > 0)
        {
            foreach (QuestSO r in questRequirements)
            {
                if (QuestManager.Instance.CheckQuestState(r.id) != QuestState.Completed)
                    canBeActive = false;
            }
        }
        else
        {
            if (canBeActive == false) return;
            canBeActive = true;
        }
    }
}
