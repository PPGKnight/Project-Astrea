using System;
using UnityEngine;

public class GatherCopperOreQuestStep : QuestStep
{
    int copperOreCollected = 0;
    int copperOreToCollect = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked += CopperOreCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked -= CopperOreCollected;
    }

    void CopperOreCollected(Item t)
    {
        if (t.GetName != "Copper Ore") return;

        if (copperOreCollected < copperOreToCollect)
            copperOreCollected += t.GetQuantity;

        if (copperOreCollected >= copperOreToCollect)
            FinishQuestStep();
    }

    void UpdateState()
    {
        string s = copperOreCollected.ToString();
        ChangeState(s);
    }

    protected override void SetQuestStepState(string s)
    {
        this.copperOreCollected = System.Int32.Parse(s);
        UpdateState();
    }
}
