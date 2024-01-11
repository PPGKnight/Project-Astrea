using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ArriveAt : QuestStep, IQuestProgress
{
    bool isVisited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
            PlaceVisited();
    }
    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ProgressQuest(questId, this);
    }

    void PlaceVisited()
    {
        isVisited = true;
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string s)
    {
        this.isVisited = System.Boolean.Parse(s);
        UpdateState();
    }

    void UpdateState()
    {
        string s = isVisited.ToString();
        ChangeState(s);
    }

    public string QuestProgress()
    {
        return "Go to a lumberjack living in the forest outside the town";
    }
}
