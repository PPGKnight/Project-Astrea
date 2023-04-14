using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    BattleData _battleData;

    [SerializeField]
    PlayerPositionSO _playerPositionSO;

    public int worldTime = 1;

    public Dictionary<string, GameObject> entity;

    [SerializeField]
    private Player player;

    public List<Player> party;

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

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<Player>();
    }

    public void ReturnToScene()
    {
        SceneManager.LoadScene(_playerPositionSO.returnToScene);
        
        //MeshRenderer o = GameObject.Find("Encounter").GetComponent<MeshRenderer>();
        //if (won) o.material.color = Color.green;
        //else o.material.color = Color.red;
        player.transform.position = _playerPositionSO.GetPlayerPosition();
        player.transform.rotation = _playerPositionSO.GetPlayerRotation();
        player.transform.localScale = _playerPositionSO.GetPlayerLocalScale();
    }

    public void BattleStart(Enemy[] enemy)
    {
        _battleData.allies.Clear();
        _battleData.enemies.Clear();
        _playerPositionSO.returnToScene = "";

        foreach(Player p in party)
        {
           _battleData.allies.Add(p);
        }
        foreach (Enemy e in enemy)
        {
            _battleData.enemies.Add(e);
        }
        GameObject _p = player.GetPosition();
        _playerPositionSO.SetPosition(_p.transform.position, _p.transform.rotation, _p.transform.localScale);
        _playerPositionSO.returnToScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Battle Arena");
    }


}