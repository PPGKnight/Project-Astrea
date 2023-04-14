using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public enum BattleState
    {
        None,
        Setup,
        Battle,
        Victory,
        Defeat
    }

public enum Turn
{
    Idle,
    Enemy,
    Ally
}

public enum TurnOptions
{
    Idle,
    Options,
    Target
}
public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    private List<Player> allies;
    private List<Enemy> enemies;
    private List<Creature> queue;

    private BattleState _battleState;
    private Turn _turn;
    private TurnOptions _turnOptions;

    [SerializeField]
    private BattleData battleData;
    [SerializeField]
    private Canvas _battleUI;
    [SerializeField]
    private GameObject _characterPanel;
    [SerializeField]
    private Canvas _battleOptions;
    //private bool isSetup = false;

    private event Action<Creature> _whoseTurn;

    private void Awake()
    {
        _battleState = BattleState.None;
        queue = new List<Creature>();
        Setup();
    }
    public void Setup()
    {
        _battleState = BattleState.Setup;
        this.allies = battleData.allies;
        this.enemies = battleData.enemies;
        GameManager.Instance.worldTime = 0;
        _whoseTurn += DoTurn;
        SetupArena();
    }

    private void SetupArena()
    {
        if (_battleState != BattleState.Setup) return;
        sbyte index = 1;
        foreach (Player player in allies)
        {
            GameObject temp = GameObject.Find("Ally" + index);
            Creature p = Instantiate(GameManager.Instance.entity[player.Name], temp.transform).GetComponent<Player>();
            p.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            p.transform.transform.localScale = new Vector3(1f, 2f, 1f);
            p.transform.parent = temp.transform;
            queue.Add(p);

            #region AllyUI
            GameObject p_characterPanel = Instantiate(_characterPanel, _battleUI.transform);
            p_characterPanel.transform.SetParent(_battleUI.transform);

            UpdateInfo uinfo = p_characterPanel.GetComponent<UpdateInfo>();
            player.Stats();
            uinfo.SetInfo(player.Name, player.Level, player.CurrentHP, player.MaxHP, player.CurrentMana, player.MaxMana);
            #endregion
            index++;
        }
        index = 1;
        foreach(Enemy enemy in enemies)
        {
            GameObject temp = GameObject.Find("Enemy" + index);
            Enemy e = Instantiate(GameManager.Instance.entity[enemy.Name], temp.transform).GetComponent<Enemy>();
            e.Name += $" {index}";
            e.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            e.transform.parent = temp.transform;
            queue.Add(e);
            index++;
        }
        Debug.Log("Jeszcze ¿yjê #1");
        //Fight();
        _battleState = BattleState.Battle;
        //isSetup = true;
    }
    #region Test-Async
    /*
    private async void Fight()
    {
        //Debug.Log("Jeszcze ¿yjê #2");
        do
        {
            foreach(GameObject c in queue)
            {
                await CheckTurn(c);
            }
            await Task.Yield();
        } while (_battleState != _battleState.Victory || _battleState != _battleState.Defeat);
        
        if(_battleState == _battleState.Victory)
        {
            Debug.Log("Walka wygrana!");
            GameManager.Instance.ReturnToScene();
        }

        if(_battleState == _battleState.Defeat)
        {
            Debug.Log("Walka przegrana!");
            GameManager.Instance.ReturnToScene();
        }
    }

    public async Task CheckTurn(GameObject g)
    {
        Player? pl = g.GetComponent<Player>();
        Enemy? e = g.GetComponent<Enemy>();
        //Debug.Log("Jeszcze ¿yjê #3");
        //Debug.Log($"Tura {c.Name}, jego inicjatywa: {c.tracker}");
        if (pl != null)
        {
            if (pl.IsDead)
            {
                allies.Remove(pl);
                queue.Remove(g);
                return;
            }

            if (_battleState != _battleState.Target)
                _battleState = _battleState.Option;

            pl.tracker += Mathf.RoundToInt((pl.Constitution + pl.Dexterity) * 0.25f + pl.SpeedBonus * Time.deltaTime);
            if (pl.tracker >= 100) await DoTurn(pl);
        }

        if (e != null)
        {
            if (e.IsDead)
            {
                enemies.Remove(e);
                queue.Remove(g);
                return;
            }

            e.tracker += Mathf.RoundToInt((e.Constitution + e.Dexterity) * 0.25f + e.SpeedBonus * Time.deltaTime);
            if (e.tracker >= 100) await DoTurn(e);
        }    

        //if (pl.GetCreatureType() == "Ally")
        //    if(_battleState != _battleState.Target)
        //        _battleState = _battleState.Option;

        //c.tracker += Mathf.RoundToInt((c.Constitution + c.Dexterity) * 0.25f + c.SpeedBonus * Time.deltaTime);
        //if (c.tracker >= 100) c.GetCreatureType() == "Ally" ? await DoTurn(c);
    }

    private void Update()
    {
        if (_battleState == _battleState.Option || _battleState == _battleState.Target) _battleOptions.gameObject.SetActive(true);
        else _battleOptions.gameObject.SetActive(false);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Klik");
            if(_battleState == _battleState.Target)
            {
                Debug.Log("Target");
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if(hit.transform.CompareTag("Enemy"))
                        //Debug.Log($"Trafiono {hit.transform.GetComponent<Enemy>().Name}");
                        

                    if(hit.transform.CompareTag("MainPlayer") || hit.transform.CompareTag("Ally"))
                        //Debug.Log($"Trafiono {hit.transform.GetComponent<Player>().Name}");

                    tempPause = false;
                }
            }
        }
    }
    
    private bool tempPause = true;
    public void SetAction(string a)
    {
        action = a;
        _battleState = _battleState.Target;
        Debug.Log($"Action set to {a}");
    }

    private string action = "";
    private Player targetAlly = null;
    private Enemy targetEnemy = null;
    private async Task DoTurn(Player c)
    {
        //Debug.Log($"Jeszcze ¿yjê #4 \n {c.GetCreatureType()}");
        targetEnemy = null;
        targetAlly = null;
        action = "";

        if (c.GetCreatureType() == "Enemy") return;
        Debug.Log(tempPause);
        if (tempPause) return;

        Debug.Log($"{targetEnemy.Name} + {action}");
            if (targetEnemy != null || targetAlly != null)
            {
            Debug.Log("Targetted");
                if(action.Length > 0)
                {
                Debug.Log("Has action");
                    switch(action){
                        case "Attack":
                            targetEnemy.TakeDamage(c.Attack());
                            print($"You attack {targetEnemy.Name} for {c.Attack()} damage!");
                            break;
                        case "Heal":
                            targetAlly.Heal(c.Intelligence);
                            print($"You heal {targetAlly.Name} for {c.Intelligence} HP");
                            break;
                        case "Guard":
                            c.Guard();
                            print($"You'll take -50% damage from the next attack made in the next turn");
                            break;
                    }
                     tempPause = true;
                }
            }

        c.tracker = 0;
        await Task.Delay(2000);
        await Task.Yield();
    }
    private async Task DoTurn(Enemy c)
    {
        //Debug.Log($"Jeszcze ¿yjê #4 \n {c.GetCreatureType()}");
        Enemy source = c;
        targetEnemy = null;
        targetAlly = null;
        action = "";

        if (c.GetCreatureType() == "Enemy") return;
        /*
        if (tempPause) return;

        if (targetEnemy != null || targetAlly != null)
        {
            if (action.Length > 0)
            {
                Debug.Log($"{targetEnemy.Name} + {action}");
                switch (action)
                {
                    case "Attack":
                        targetEnemy.TakeDamage(source.Attack());
                        print($"You attack {targetEnemy.Name} for {source.Attack()} damage!");
                        break;
                    case "Heal":
                        targetAlly.Heal(source.Intelligence);
                        print($"You heal {targetAlly.Name} for {source.Intelligence} HP");
                        break;
                    case "Guard":
                        source.Guard();
                        print($"You'll take -50% damage from the next attack made in the next turn");
                        break;
                }
            }
        }
        

        c.tracker = 0;
        await Task.Delay(1000);
        await Task.Yield();
    }*/
    #endregion

    private string action = "";
    private GameObject target = null;
    private bool isSomeonesTurn = false;
    int alliesDeaths = 0;
    int enemiesDeaths = 0;

    void Fight()
    {
        Debug.Log(isSomeonesTurn);
        if (isSomeonesTurn) return;

        foreach(Creature c in queue)
        {
            c.tracker += Mathf.RoundToInt(((c.Dexterity + c.Strength) * 0.25f + c.SpeedBonus)); 
            Debug.Log($"{c.Name} posiada {c.tracker} / 100 inicjatywy!");
            bool a = (c.tracker >= 100);
            Debug.Log($" Wiêcej od 100? {c.tracker} {a}");
            if(c.tracker >= 100f)
            {
                if (c.GetCreatureType() == "Enemy") { _turn = Turn.Enemy; c.tracker = 0; continue; }
                _turn = Turn.Ally;
                isSomeonesTurn = true;
                _whoseTurn?.Invoke(c);

            }
        }
    }

    public void SetAction(string s)
    {
        action = s;
        Debug.Log($"Ustawiono akcjê na {action}");
        _turnOptions = TurnOptions.Target;
    }

    public void CheckDeaths()
    {

        foreach(Creature c in queue)
        {            
            if (c.IsDead()) { 
                Debug.Log($"{c.Name} zosta³ zg³adzony!");
                if (c.GetCreatureType() == "Enemy") enemiesDeaths++;
                else alliesDeaths++;

                queue.Remove(c);
                c.gameObject.SetActive(false);
                Debug.Log($"{alliesDeaths} + {enemiesDeaths}");
            }

            if (enemiesDeaths == enemies.Count) {
                Debug.Log("Uda³o Ci siê wygraæ bitwê!");
                _battleState = BattleState.Victory;
                GameManager.Instance.ReturnToScene();
            }
            else if (alliesDeaths == allies.Count)
            {
                Debug.Log("Niestety przegra³eœ tê walkê!");
                _battleState = BattleState.Defeat;
                GameManager.Instance.ReturnToScene();
            }


            Task.Delay(2000);
        }
    }

    private void Update()
    {
        if (_battleState == BattleState.Battle)
        {
            CheckDeaths();
            if (!isSomeonesTurn) Fight();
            if (isSomeonesTurn && _turn == Turn.Ally && action.Length <= 0) _turnOptions = TurnOptions.Options;
            if ((_turnOptions == TurnOptions.Options || _turnOptions == TurnOptions.Target))
            {
                _battleOptions.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log($"Klik {_turnOptions}");
                    if (_turnOptions == TurnOptions.Target)
                    {
                        Debug.Log("Target");
                        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform.CompareTag("Enemy"))
                                Debug.Log($"Trafiono {hit.transform.GetComponent<Enemy>().Name}");
                            if (hit.transform.CompareTag("MainPlayer") || hit.transform.CompareTag("Ally"))
                                Debug.Log($"Trafiono {hit.transform.GetComponent<Player>().Name}");

                            target = hit.transform.gameObject;
                        }
                    }
                    if (action.Length > 0 && target != null) ManualCheck();
                }
            }
            else _battleOptions.gameObject.SetActive(false);
        }
    }

    void ManualCheck()
    {
        foreach(Creature c in queue)
        {
            if (c.tracker >= 100) DoTurn(c);
        }
        //isSomeonesTurn = false;
    }

    void DoTurn(Creature c)
    {
        if (action.Length > 0 && target != null)
        {
            Debug.Log($"Jestem w DoTurn z obiektem {c.Name}!");
            switch (action)
            {
                case "Attack":
                    print($"You attacked {target.GetComponent<Enemy>().Name} for {c.GetComponent<Player>().Attack()} damage!");
                    target.GetComponent<Enemy>().TakeDamage(c.GetComponent<Player>().Attack());
                    break;
                case "Heal":
                    print($"You heal {target.GetComponent<Player>().Name} for {c.GetComponent<Player>().Intelligence} health points!");
                    target.GetComponent<Player>().Heal(c.GetComponent<Player>().Intelligence);
                    break;
                case "Guard":
                    print($"You will take -50% damage on enemy's next attack");
                    c.GetComponent<Player>().Guard();
                    break;
            }

            target = null;
            action = "";
            _turn = Turn.Idle;
            _turnOptions = TurnOptions.Idle;
            isSomeonesTurn = false;
        }
    }
}
