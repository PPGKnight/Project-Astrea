using System;
using UnityEngine;

public class GatherItemsQuestStep : QuestStep
{
    int flintCollected = 0;
    int flintToCollect = 5;

    int copperCollected = 0;
    int copperToCollect = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked += ItemCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.MiscEvents.onItemPicked -= ItemCollected;
    }

    void ItemCollected(Item t)
    {
        Debug.LogWarning(t.GetName);
        if (t.GetName != "Flint" && t.GetName != "Copper Ore") return;

        if(t.GetName == "Flint")
            if (flintCollected < flintToCollect)
                flintCollected += t.GetQuantity;

        if (t.GetName == "Copper Ore")
            if (copperCollected < copperToCollect)
                copperCollected += t.GetQuantity;
        
        if (flintCollected >= flintToCollect && copperCollected >= copperToCollect)
            FinishQuestStep();
    }

    void UpdateState()
    {
        string s = flintCollected.ToString() + "|" + copperCollected.ToString();
        ChangeState(s);
    }

    protected override void SetQuestStepState(string s)
    {
        string[] e = s.Split("|");
        this.flintCollected = System.Int32.Parse(e[0]);
        this.copperCollected = System.Int32.Parse(e[1]);
        UpdateState();
    }
}
