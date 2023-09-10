using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ArriveAt : QuestStep
{
    bool isVisited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
            PlaceVisited();
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
}
