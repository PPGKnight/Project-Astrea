using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    List<Creature> _trackerInOrder;
    int alliesAlive = 0;
    int enemiesAlive = 0;

    public TurnManager(List<Creature> tracker)
    {
        _trackerInOrder = tracker;
        foreach (Creature c in tracker)
            _ = c.GetCreatureType() == "Ally" ? alliesAlive++ : enemiesAlive++;

        Reorder();
    }

    public Creature GetCreature() => _trackerInOrder[0];

    public void NextTurn()
    {
        Creature temp = _trackerInOrder[0];
        temp.InitiativeThisFight += 20;
        _trackerInOrder.RemoveAt(0);
        _trackerInOrder.Add(temp);
    }


    public void Reorder() => _trackerInOrder.Sort((x, y) => x.InitiativeThisFight.CompareTo(y.InitiativeThisFight));

    public void RemoveCreature(Creature c) => _trackerInOrder?.Remove(c);

    public void CheckDeaths()
    {
        bool anyDead = false;
        foreach(Creature c in _trackerInOrder)
        {
            if (c.IsDead())
            {
                anyDead = true;
                _ = c.GetCreatureType() == "Ally" ? alliesAlive-- : enemiesAlive--;
                if(c.GetCreatureType() == "Enemy") Destroy(c.entityInfo.gameObject);
                //Destroy(c.gameObject);
                Animator a = c.GetComponent<Animator>();
                a.applyRootMotion = true;
                a.Play("Stunned");
            }
        }
        _trackerInOrder.RemoveAll(c => c.IsDead());
        if(anyDead) BattleManager.Instance.RemoveDead();
    }

    public Tuple<bool, int> CheckResults()
    {
        if(alliesAlive == 0)
            return new Tuple<bool, int>(true,1);

        if(enemiesAlive == 0)
            return new Tuple<bool, int>(true, 2);

        return new Tuple<bool, int>(false, 0);
    }

    public List<Creature> GetTracker() => _trackerInOrder;
}
