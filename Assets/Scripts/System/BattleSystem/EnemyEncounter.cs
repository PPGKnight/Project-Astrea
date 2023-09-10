using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    public string encounterID;
    public Enemy[] enemies;
    public string arenaName;

    bool isReady = false;

    [SerializeField]
    GameObject DialogueAfterEncounter = null;

    [SerializeField]
    private EncounterType encounterType;

    private void Start()
    {
        CheckReady();
        //CheckDialogue();
    }

    private void CheckReady()
    {
        if(!EncounterList.Instance.GetEncounter(encounterID) && isReady)
            this.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        else
            this.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }

    private void CheckDialogue()
    {
        if (!EncounterList.Instance.GetEncounter(encounterID)) return;
        if (DialogueManager.Instance.dialogueList.CheckIfCompleted(DialogueAfterEncounter.name)) return;
    }

    public void UpdateReady()
    {
        isReady = true;
        CheckReady();
    }

    private void OnValidate()
    {
        encounterID = gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (encounterType != EncounterType.WhenInRange) return;

        if (other.gameObject.CompareTag("MainPlayer"))
        {
            GameManager.Instance.BattleStart(enemies, encounterID, arenaName);
        }
    }
}