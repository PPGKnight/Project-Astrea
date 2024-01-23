using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] string Description;
    [SerializeField] bool CanWear;
    [SerializeField] bool CanStack;
    [SerializeField] int StackSize;
    [SerializeField] int Quantity = 1;
    [SerializeField] bool pickWithoutAdding;

    [SerializeField] DialogueTriggerType inter;
    [SerializeField] GameObject DialogueIfAllReqsAreMet;
    [SerializeField] GameObject DialogueIfAllReqsAreNotMet;

    [SerializeField] List<string> NeededItems;
    bool PlayerInRange = false;
    bool CanPickUp = false;

    private void Awake()
    {
        if(DialogueIfAllReqsAreMet != null)
            DialogueIfAllReqsAreMet.SetActive(false);

        if(DialogueIfAllReqsAreNotMet != null)
            DialogueIfAllReqsAreNotMet.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerMovement.InteractionWithNPC += ListenForInput;
        PlayerMovement.InteractionWithMouse += ListenForMouseInput;
    }
    private void OnDisable()
    {
        PlayerMovement.InteractionWithNPC -= ListenForInput;
        PlayerMovement.InteractionWithMouse -= ListenForMouseInput;
    }

    void ListenForInput()
    {
        if (PlayerInRange == false) return;

        CanPickUp = true;
        foreach (string s in NeededItems)
            if (!GameManager.Instance.Inventory.Contains(s))
                CanPickUp = false;

        if (!CanPickUp)
        {
            StopAllCoroutines();
            StartCoroutine(DisableDialogObject(DialogueIfAllReqsAreNotMet));
            return;
        }

        if(DialogueIfAllReqsAreMet != null) 
            DialogueIfAllReqsAreMet.SetActive(true);

        PickUpItem();
    }

    void ListenForMouseInput(GameObject go)
    {
        if (go.transform.parent.parent == this.gameObject)
            ListenForInput();
    }

    IEnumerator DisableDialogObject(GameObject gObject)
    {
       if (gObject == null) Debug.Log($"GameObject's missing in {gameObject.name}'s component");
       gObject.SetActive(true);
       yield return new WaitForSeconds(1f);
       gObject.SetActive(false);
    }

    public string GetName { get { return Name; } }
    public int GetQuantity { get { return Quantity; } }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Kolizja z {Name} w liczbie {Quantity}");

        if (other.CompareTag("MainPlayer") && inter == DialogueTriggerType.InRange)
        {
            PickUpItem();
        }
        else if (other.CompareTag("MainPlayer") && inter == DialogueTriggerType.ByInteraction)
            PlayerInRange = true;

    }

    public void SetItem(string name = "default", string desc = "default")
    {
        Name = name;
        Description = desc;
    }

    void PickUpItem()
    {
        if(!pickWithoutAdding)
        {
            GameEventsManager.instance.MiscEvents.ItemPicked(this);
            GameManager.Instance.Inventory.Add(Name);
        }
        Destroy(this.gameObject);
    }
}