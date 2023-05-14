using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private GameObject _player;

    private Player player;

    public List<Player> party;

    [SerializeField]
    private GameObject[] encounters;

    public static GameManager Instance { 
        get { return _instance; } 
    }

    private void Awake()
    {
        _ = SpawnPlayer();
        if(_instance == null)
        {
            _instance = this;
            _player.transform.position = Vector3.zero;
        }
        else
            Destroy(this.gameObject);

        Populate();
        party.Add(Resources.Load<Player>("Prefabs/PlayerModel"));

        encounters = GameObject.FindGameObjectsWithTag("Encounter");

        if (SceneManager.GetActiveScene().name == "Testing Ground")
            foreach (GameObject o in encounters)
                if (o.name == _battleData.eID.ToString())
                    o.SetActive(false);

        DontDestroyOnLoad(this);
    }
    private async Task SpawnPlayer()
    {
        if (_player == null)
        {
            _player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), _playerPositionSO.GetPlayerPosition(), Quaternion.identity);
            player = _player.GetComponent<Player>();
        }
        await Task.Run(() =>
        {
            Task.Delay(20);
        });
        player.Stats();
    }
    #region LoadPlayer
    //private async Task LoadPlayer()
    //{
    //    await Task.Delay(2000);
    //    _player = null;
    //    player = null;
    //    if (_player == null)
    //    {
    //        _player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), new Vector3(0f, 0f, 0f), Quaternion.identity);
    //        player = _player.GetComponent<Player>();
    //    }
    //    //_player.transform.position = _playerPositionSO.GetPlayerPosition();
    //    _player.transform.position = new Vector3(-6f, 2.17f, 2f);
    //    _player.transform.rotation = _playerPositionSO.GetPlayerRotation();
    //    _player.transform.localScale = _playerPositionSO.GetPlayerLocalScale();
    //}
    #endregion
    public void ReturnToScene()
    {
        SceneManager.LoadScene(_playerPositionSO.returnToScene);
        //_ = LoadPlayer();
    }

    public void Check()
    {
        
        Debug.Log($"Encounter_{_battleData.eID}");
        GameObject.Find($"Encounter_{_battleData.eID}").SetActive(false);
    }

    public void BattleStart(Enemy[] enemy, int eid)
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
        //GameObject _p = player.GetPosition();
        _playerPositionSO.SetPosition(_player.transform.position, _player.transform.rotation, _player.transform.localScale);
        _playerPositionSO.returnToScene = SceneManager.GetActiveScene().name;
        _battleData.eID = eid;
        player = null;
        _player = null;
        SceneManager.LoadScene("Battle Arena");
    }

    public void Populate()
    {
        entity = new Dictionary<string, GameObject>()
        {
            {"Gracz", Resources.Load<GameObject>("Prefabs/PlayerModel")},
            {"Big Bad", Resources.Load<GameObject>("Prefabs/Enemy")}
        };
    }
}