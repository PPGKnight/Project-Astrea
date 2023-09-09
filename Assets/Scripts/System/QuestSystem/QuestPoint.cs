using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        CheckQuestStatus();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.onQuestStateChange += QuestStateChange;
        PlayerMovement.InteractionWithNPC += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.onQuestStateChange -= QuestStateChange;
        PlayerMovement.InteractionWithNPC -= SubmitPressed;
    }

    void CheckQuestStatus()
    {
        //if (QuestManager.Instance.CheckQuestState(questId) == QuestState.Completed)
            questIcon.SetState(QuestManager.Instance.CheckQuestState(questId), startPoint, finishPoint);
    }

    private void SubmitPressed()
    {
        if (!playerIsNear) return;
        Debug.Log("Submit nacisniety");

        // start or finish a quest
        if (currentQuestState.Equals(QuestState.Can_Start) && startPoint)
        {
            Debug.Log("Rozpoczynam Quest");
            GameEventsManager.instance.QuestEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.Can_Finish) && finishPoint)
        {
            Debug.Log("Koncze Quest");
            GameEventsManager.instance.QuestEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("MainPlayer"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("MainPlayer"))
        {
            playerIsNear = false;
        }
    }
}