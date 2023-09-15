using System;
using UnityEngine;

public class GatherFlintQuestStep : QuestStep
{
    int flintCollected = 0;
    int flintToCollect = 5;

    private void OnEnable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked += FlintCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked -= FlintCollected;
    }

    void FlintCollected(Item t)
    {
        if (t.GetName != "Flint") return;

        if (flintCollected < flintToCollect)
            flintCollected += t.GetQuantity;

        if (flintCollected >= flintToCollect)
            FinishQuestStep();
    }

    void UpdateState()
    {
        string s = flintCollected.ToString();
        ChangeState(s);
    }

    protected override void SetQuestStepState(string s)
    {
        this.flintCollected = System.Int32.Parse(s);
        UpdateState();
    }
}
