using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private BattleManager _battleManager;
    public int worldTime = 1;
    public List<Player> party;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerCam;

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

        DontDestroyOnLoad(this);
        Instantiate(player);
        Instantiate(playerCam);

        SceneManager.LoadScene("Battle Arena", LoadSceneMode.Additive);
    }


    public void BattleStart(Enemy[] enemy)
    {
        Debug.Log("Wczytujê walkê");
        _battleManager = ScriptableObject.CreateInstance<BattleManager>();
        foreach(Player p in party)
        {
           _battleManager.allies.Add(p);
        }
        foreach (Enemy e in enemy)
        {
            _battleManager.enemies.Add(e.gameObject);
        }
        
        //SceneManager.LoadScene("Battle Arena", LoadSceneMode.Additive);
        _battleManager.Battle();

    }


}