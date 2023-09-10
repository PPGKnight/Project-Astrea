using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject vIndicator;
    [SerializeField] TextAsset inkStory;
    [SerializeField] DialogueTriggerType dialogueTriggerType;
    [SerializeField] bool oneTimeDialogue = true;
    GameObject parent;

    DialogueManager manager;

    bool inDialogue = false;
    bool playerInRange = false;
    bool isInteracting = false;
    private void Awake()
    {
        parent = gameObject.transform.parent.parent.gameObject;
        Debug.Log(parent.name);

        if (manager == null)
        {
            Debug.LogWarning("Manager wyjebalo awake");
            manager = DialogueManager.Instance;
        }

        playerInRange = false;
        vIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerMovement.InteractionWithNPC += ListenForInput;
    }
    private void OnDisable()
    {
        PlayerMovement.InteractionWithNPC -= ListenForInput;
    }

    void ListenForInput()
    {
        StopAllCoroutines();
        StartCoroutine(EmitKey());
    }

    IEnumerator EmitKey()
    {
        isInteracting = true;
        yield return new WaitForSeconds(1f);
        isInteracting = false;
    }

    private void Update()
    {
        //if (oneTimeDialogue && DialogueManager.Instance.dialogueList.CheckIfCompleted(parent.name))
        //    parent.SetActive(false);
        if (manager == null)
        {
            Debug.LogWarning("Manager wyjebalo update");
            manager = DialogueManager.Instance;
        }

        if (inDialogue) return;

        if (dialogueTriggerType == DialogueTriggerType.InRange) TriggerBeingNear();
        else if (dialogueTriggerType == DialogueTriggerType.ByInteraction) TriggerKey();
    }

    void TriggerKey()
    {
        if (playerInRange && !DialogueManager.Instance.isDialogue)
        {
            if (manager == null)
            {
                Debug.LogWarning("Manager wyjebalo triggerkey");
                manager = DialogueManager.Instance;
            }

            if (oneTimeDialogue)
                if (manager.dialogueList.CheckIfCompleted(parent.name))
                    return;
            vIndicator.SetActive(true);
            if (isInteracting)
            {
                inDialogue = true;
                manager.EnterDialogue(inkStory);
            }
        }
        else
            vIndicator.SetActive(false);
    }

    void TriggerBeingNear()
    {

        if (playerInRange && !DialogueManager.Instance.isDialogue)
        {
            if (manager == null)
            {
                Debug.LogWarning("Manager wyjebalo triggernear");
                manager = DialogueManager.Instance;
            }

            if (oneTimeDialogue)
                if (manager.dialogueList.CheckIfCompleted(parent.name))
                    return;

            Debug.Log("Uruchamiam dialog");
            inDialogue = true;

            if (inkStory == null) Debug.LogError("O chuj");
            manager.EnterDialogue(inkStory);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainPlayer") playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MainPlayer") playerInRange = false;
    }
}
