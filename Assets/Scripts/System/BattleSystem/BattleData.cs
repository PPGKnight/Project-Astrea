using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestAsset/Battle Data")]
public class BattleData : ScriptableObject
{
    #nullable enable
    public string? eID;
    #nullable disable
    public List<Player> allies;
    public List<Enemy> enemies;
    public BattleStatus battleStatus;
}