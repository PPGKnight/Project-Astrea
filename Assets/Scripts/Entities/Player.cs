using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public void Stats()
    {
        //GameManager.Instance.party.Add(this);
        base.UpdateStats();
    }

    public void UpdateMyStats()
    {
        base.UpdateStats();
    }
}
