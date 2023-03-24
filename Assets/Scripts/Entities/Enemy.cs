using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    private void Awake()
    {
        base.UpdateStats();
    }
}
