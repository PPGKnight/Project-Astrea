using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestAsset/Battle Data")]
public class BattleData : ScriptableObject
{
    public string? eID;
    public List<Player> allies;
    public List<Enemy> enemies;
    public BattleStatus battleStatus;
}

public enum BattleStatus
{
    Defeat,
    Victory
}
