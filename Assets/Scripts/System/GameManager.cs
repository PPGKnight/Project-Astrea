using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    BattleData _battleData;

    public int worldTime = 1;

    public Dictionary<string, GameObject> entity;

    [SerializeField]
    private Player player;

    public List<Player> party;
    public PlayerPositionSO playerPosition;

    public static GameManager Instance { 
        get { return _instance; } 
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this.gameObject);

        entity = new Dictionary<string, GameObject>()
        {
            {"Gracz", Resources.Load<GameObject>("Prefabs/PlayerModel")},
            {"Big Bad", Resources.Load<GameObject>("Prefabs/Enemy")}
        };
        party.Add(Resources.Load<Player>("Prefabs/PlayerModel"));
        player.Stats();
        DontDestroyOnLoad(this);
    }


    public void BattleStart(Enemy[] enemy)
    {
        _battleData.allies.Clear();
        _battleData.enemies.Clear();

        foreach(Player p in party)
        {
           _battleData.allies.Add(p);
        }
        foreach (Enemy e in enemy)
        {
            _battleData.enemies.Add(e);
        }
        
        SceneManager.LoadScene("Battle Arena");
    }


}