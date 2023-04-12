using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestAsset/Battle Data")]
public class BattleData : ScriptableObject
{
    public List<Player> allies;
    public List<Enemy> enemies;
    public string returnToScene;
}
