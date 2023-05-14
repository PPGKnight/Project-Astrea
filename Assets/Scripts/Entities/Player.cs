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

    public GameObject GetPosition()
    {
        return gameObject.transform.parent.gameObject;
    }

    public void UpdateMyStats()
    {
        base.UpdateStats();
    }
}
