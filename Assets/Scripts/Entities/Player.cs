using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    private void Awake()
    {
        GameManager.Instance.party.Add(this);
        base.UpdateStats();
    }
}
