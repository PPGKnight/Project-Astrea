using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestSO questInfoForPoint;
    [SerializeField] private List<QuestSO> requiredQuests;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    [SerializeField] private SendAnalyticsData data;

    private bool allRequiredCompleted = true;
    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        currentQuestState = QuestManager.Instance.CheckQuestState(questId);
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

        allRequiredCompleted = true;

        if(requiredQuests != null)
            foreach (QuestSO q in requiredQuests)
                if (QuestManager.Instance.CheckQuestState(q.id) != QuestState.Completed)
                    allRequiredCompleted = false;

        if (allRequiredCompleted)
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        else
            questIcon.SetState(QuestState.Hidden, startPoint, finishPoint);
    }

    private void SubmitPressed()
    {
        if (!playerIsNear) return;
        Debug.Log("Submit nacisniety");
        
        Debug.Log($"{QuestManager.Instance.CheckQuestState(questId)} -> {startPoint} -> {allRequiredCompleted}");
        if (currentQuestState.Equals(QuestState.Can_Start) && startPoint)
        {
            Debug.Log("Rozpoczynam Quest");
            GameEventsManager.instance.QuestEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.Can_Finish) && finishPoint)
        {
            Debug.Log("Koncze Quest");
            GameEventsManager.instance.QuestEvents.FinishQuest(questId);
            if (data == SendAnalyticsData.Yes)
                AnalyticsManager.Instance.SentAnalyticsData(AnalyticsDataEvents.QuestCompleted, questInfoForPoint.id);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            CheckQuestStatus();
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("MainPlayer"))
            playerIsNear = true;
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("MainPlayer"))
            playerIsNear = false;
    }
}

public enum SendAnalyticsData
{
    No,
    Yes
}