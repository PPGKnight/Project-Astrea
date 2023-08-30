using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject vIndicator;
    [SerializeField] TextAsset inkStory;

    bool playerInRange = false;
    bool isInteracting = false;
    private void Awake()
    {
        playerInRange = false;
        vIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerMovement.InteractionWithNPC += ListenForInput;
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
        if (playerInRange && !DialogueManager.Instance.isDialogue)
        {
            vIndicator.SetActive(true);
            if (isInteracting)
                DialogueManager.Instance.EnterDialogue(inkStory);
        }
        else
            vIndicator.SetActive(false);
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
