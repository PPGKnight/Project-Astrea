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
    public PlayerPositionSO _playerPositionSO;

    public int worldTime = 1;

    public Dictionary<string, GameObject> entity;

    [SerializeField]
    public GameObject _player;

    private Player player;

    [SerializeField]
    public List<string> activeScenes;

    public List<Player> party;

    [SerializeField]
    private GameObject[] encounters;

    public static GameManager Instance { 
        get { return _instance; } 
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            //ascene = SceneManager.GetActiveScene().name;
            //_player.transform.position = Vector3.zero;
        }
        else
            Destroy(this.gameObject);

        SpawnThePlayer(false);
        Populate();
        party.Add(Resources.Load<Player>("Prefabs/PlayerModel"));

        encounters = GameObject.FindGameObjectsWithTag("Encounter");

        activeScenes.Add(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "Testing Ground")
            foreach (GameObject o in encounters)
                if (o.name == _battleData.eID.ToString())
                    o.SetActive(false);

        DontDestroyOnLoad(this);
    }
    public void SpawnThePlayer(bool keepPosition)
    {
        _ = SpawnPlayer(keepPosition);
    }
    public async Task SpawnPlayer(bool keepPosition)
    {
        if (_player == null)
        {
            _player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), keepPosition ? _playerPositionSO.GetPlayerPosition() : _playerPositionSO.spawnPoint[SceneManager.GetActiveScene().name], Quaternion.identity);
            player = _player.GetComponent<Player>();
        }
        await Task.Run(() =>
        {
            Task.Delay(20);
        });
        player.Stats();
    }
    public void ReturnToScene()
    {
        SceneManager.LoadScene(_playerPositionSO.returnToScene);
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

    public void ChangePlayerPos(Vector3 newPos)
    {
        Debug.Log(newPos);
        _playerPositionSO.SetOnlyPosition(newPos);
    }

    public void MovePlayerToNewPos()
    {
        Debug.Log(_playerPositionSO.GetPlayerPosition());
        _player.transform.position.Set(_playerPositionSO.GetPlayerPosition().x, _playerPositionSO.GetPlayerPosition().y, _playerPositionSO.GetPlayerPosition().z);
    }
}