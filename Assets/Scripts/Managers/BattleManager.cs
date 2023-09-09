using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    List<Player> allies;
    List<Enemy> enemies;
    List<Creature> queue;
    Queue<Creature> _queue;
    [SerializeField] public Animator animator;

    BattleState _battleState;
    Turn _turn;
    TurnOptions _turnOptions;

    public FloatingNumbers _floatingNumbers;
    public InitiativeTrackerManager initiativeTrackerManager;

    public static event Action ProgressTurn;
    public static event Action UpdateBars;

    [SerializeField] BattleData battleData;
    [SerializeField] Canvas _battleUI;
    [SerializeField] GameObject _characterPanel;
    [SerializeField] GameObject _enemyPanel;
    [SerializeField] Canvas _battleOptions;
    Creature creatureThisTurn;

    #region Setup
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
        SetupArena();
    }

    private void SetupArena()
    {
        if (_battleState != BattleState.Setup) return;
        sbyte index = 1;
        foreach (Player player in allies)
        {
            animator = GetComponent<Animator>();
            GameObject temp = GameObject.Find("Ally" + index);
            Creature p = Instantiate(GameManager.Instance.entity[player.Name], temp.transform).GetComponent<Player>();
            p.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            p.transform.transform.localScale = new Vector3(1f, 2f, 1f);
            p.transform.parent = temp.transform;
            queue.Add(p);

            /*
            #region AllyUI
            GameObject p_characterPanel = Instantiate(_characterPanel, _battleUI.transform);
            p_characterPanel.transform.SetParent(_battleUI.transform);
            p_characterPanel.transform.localPosition = new Vector3(1075f - (index*325f), -420f, 0f);

            p.entityInfo = p_characterPanel.GetComponent<UpdateInfo>();
            p.entityInfo.SetInfo(p.Name, p.Level, p.CurrentHP, p.MaxHP);
            #endregion
            */
            index++;
        }
        index = 1;
        foreach(Enemy enemy in enemies)
        {
            animator = GetComponent<Animator>();
            GameObject temp = GameObject.Find("Enemy" + index);
            Enemy e = Instantiate(GameManager.Instance.entity[enemy.Name], temp.transform).GetComponent<Enemy>();

            
            GameObject e_characterPanel = Instantiate(_enemyPanel, temp.transform);
            e.entityInfo = e_characterPanel.GetComponent<UpdateInfo>();
            e.entityInfo.SetEnemyInfo(e.Name, e.CurrentHP, e.MaxHP);
            

            e.Name += $" {index}";
            e.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            e.transform.parent = temp.transform;
            queue.Add(e);
            index++;
        }

        foreach (Creature q in queue)
            q.SetInitiative();

        queue.Sort((x, y) => y.InitiativeThisFight.CompareTo(x.InitiativeThisFight));
        _queue = new Queue<Creature>(queue);

        CreateTokens();
        _battleState = BattleState.Battle;
        StartNextTurn();
    }

    void CreateTokens()
    {
        initiativeTrackerManager.CreateTokens(queue);
    }

    private string action = "";
    private GameObject target = null;
    private bool isSomeonesTurn = false;
    int alliesDeaths = 0;
    int enemiesDeaths = 0;
    #endregion

    #region WorkingWithDelay
    /*
    void Fight()
    {
        if (isSomeonesTurn) return;

        foreach(Creature c in queue)
        {
            if (isSomeonesTurn) return;
            c.tracker += Mathf.RoundToInt(((c.Dexterity + c.Strength) * 0.05f + c.SpeedBonus)); 
            ProgressTurn();
            if(c.tracker >= 100f)
            {
                if (c.HadTurn) continue;
                Debug.Log($"Tura: {c.Name}");
                c.tracker = 100f;
                if (c.GetCreatureType() == "Enemy") { _turn = Turn.Enemy; DoEnemyTurn(c); }
                else AllyTurn();

            }
        }
        foreach (Creature c in queue) c.HadTurn = false;
    }
    */

    public void StartNextTurn()
    {
        if (_battleState == BattleState.Victory || _battleState == BattleState.Defeat) return;
        CheckDeaths();
        if(!isSomeonesTurn && _queue.Count > 0)
        {
            ProgressTurn?.Invoke();
            creatureThisTurn = _queue.Dequeue();
            if (creatureThisTurn.IsDead() || creatureThisTurn == null) StartNextTurn();
            Debug.Log($"Tura {creatureThisTurn.Name}");
            if (creatureThisTurn.GetCreatureType() == "Enemy")
            {
                _turn = Turn.Enemy;
                DoEnemyTurn(creatureThisTurn);
            }
            else
                AllyTurn();
            
            if(UpdateBars != null)
                UpdateBars();
        }
        else if(_queue.Count <= 0)
        {
            _queue = new Queue<Creature>(queue);
            StartNextTurn();
        }
    }

    public void SetAction(string s)
    {
        action = s;
        Debug.Log($"Ustawiono akcje na {action}");
        _turnOptions = TurnOptions.Target;
    }

    public void CheckDeaths()
    {
        for(int t = queue.Count - 1; t >= 0; t--)
        {        
            Creature c = queue[t];
            if (c.IsDead()) { 
                Debug.Log($"{c.Name} zostal zgladzony!");
                if (c.GetCreatureType() == "Enemy") enemiesDeaths++;
                else alliesDeaths++;

                initiativeTrackerManager.RemoveToken();
                queue.Remove(c);
                Destroy(c.entityInfo.gameObject);
                Destroy(c.gameObject);
            }
            
            if (enemiesDeaths == enemies.Count) {
                Debug.Log("Udalo Ci sie wygrac bitwe!");
                _battleState = BattleState.Victory;
                battleData.battleStatus = BattleStatus.Victory;
                LeaveBattle();
            }
            else if (alliesDeaths == allies.Count)
            {
                Debug.Log("Niestety przegrales te walke!");
                _battleState = BattleState.Defeat;
                battleData.battleStatus = BattleStatus.Defeat;
                LeaveBattle();
            }
        }
    }

    void LeaveBattle()
    {
        StopAllCoroutines();
        GameManager.Instance.ReturnToScene();
    }

    private void Update()
    {
        if (isSomeonesTurn && _turn == Turn.Ally && action.Length <= 0) _turnOptions = TurnOptions.Options;
        
        if ((_turnOptions == TurnOptions.Options || _turnOptions == TurnOptions.Target))
                _battleOptions.gameObject.SetActive(true);
        else _battleOptions.gameObject.SetActive(false);
        
    }

    void SelectOptions()
    {
        if (_battleState != BattleState.Battle) return;
        
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
            StartCoroutine(DoTurn(creatureThisTurn));
            _turnOptions = TurnOptions.Idle;
            }
        }
    }

    void AllyTurn()
    {
        Debug.Log("Wykonalem sie");
        isSomeonesTurn = true;
        _turn = Turn.Ally;
        action = "";
        target = null;
    }

    IEnumerator DoTurn(Creature c)
    {
        if (action.Length > 0 && target != null)
        {
            FloatingNumbers f;
            switch (action)
            {
                case "Attack":
                    int a = c.GetComponent<Player>().Attack();
                    yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isAttacking"));
                    yield return StartCoroutine(BeginAnimation(target.GetComponent<Animator>(), "isReacting"));
                    print($"You attacked {target.GetComponent<Enemy>().Name} for {a} damage!");
                    target.GetComponent<Enemy>().TakeDamage(a);
                    f = Instantiate(_floatingNumbers, target.transform.position, Quaternion.identity);
                    f.SetText(a, Color.red);
                    target.GetComponent<Enemy>().entityInfo.UpdateHP(target.GetComponent<Enemy>().CurrentHP);
                    break;
                case "Heal":
                    print($"You heal {target.GetComponent<Player>().Name} for {c.GetComponent<Player>().Intelligence} health points!");
                    yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isHealing"));
                    target.GetComponent<Player>().Heal(c.GetComponent<Player>().Intelligence);
                    f = Instantiate(_floatingNumbers, target.transform.position, Quaternion.identity);
                    f.SetText(c.Intelligence, Color.green);
                    //target.GetComponent<Player>().entityInfo.UpdateHP(target.GetComponent<Player>().CurrentHP);
                    break;
                case "Guard":
                    print($"You will take -50% damage on enemy's next attack");
                    yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isGuarding"));
                    c.GetComponent<Player>().Guard();
                    f = Instantiate(_floatingNumbers, target.transform.position, Quaternion.identity);
                    f.SetText("Block!", Color.blue);
                    break;
            }

            target = null;
            action = "";
            _turn = Turn.Idle;
            c.tracker = 0;
            isSomeonesTurn = false;
            StartNextTurn();
        }
    }

    void DoEnemyTurn(Creature c)
    {
        if(!isSomeonesTurn)
            StartCoroutine(DelayEnemyTurn(c)); 
    }

    IEnumerator DelayEnemyTurn(Creature c)
    {
        isSomeonesTurn = true;
        _turnOptions = TurnOptions.Idle;
        yield return new WaitForSeconds(0.5f);

        List<Creature> insideAllies = queue.Where(t => t.GetCreatureType() == "Ally").ToList();
        List<Creature> insideEnemies = queue.Where(t => t.GetCreatureType() == "Enemy").ToList();

        System.Random rnd = new System.Random();
        FloatingNumbers f;

        switch (rnd.Next(0, 3))
        {
            case 0:
                int att = rnd.Next(0, insideAllies.Count);
                int a = c.Attack();
                yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isAttacking"));
                Debug.Log($"{c.Name} atakuje {insideAllies[att].Name} za {a} punktow obrazen!");
                insideAllies[att].TakeDamage(a);
                yield return StartCoroutine(BeginAnimation(insideAllies[att].GetComponent<Animator>(), "isReacting"));
                f = Instantiate(_floatingNumbers, insideAllies[att].transform.position, Quaternion.identity);
                f.SetText(a, Color.red);
                //insideAllies[att].entityInfo.UpdateHP(insideAllies[att].CurrentHP);
                break;
            case 1:
                insideEnemies.Sort((a, b) => a.CurrentHP.CompareTo(b.CurrentHP));
                Debug.Log($"{c.Name} leczy {insideEnemies[0].Name} za {c.Intelligence} punktow zdrowia");
                yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isHealing"));
                insideEnemies[0].Heal(c.Intelligence);
                f = Instantiate(_floatingNumbers, insideEnemies[0].transform.position, Quaternion.identity);
                f.SetText(c.Intelligence, Color.green);
                //insideEnemies[0].entityInfo.UpdateHP(insideEnemies[0].CurrentHP);
                break;
            case 2:
                //c.GetComponent<Animator>().SetBool("isGuarding", true);
                yield return StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isGuarding"));
                Debug.Log($"{c.Name} broni sie przez co ten otrzyma o polowe mniej obrazen");
                f = Instantiate(_floatingNumbers, insideEnemies[0].transform.position, Quaternion.identity);
                f.SetText("Block!", Color.blue);
                c.Guard();
                break;
        }
        c.tracker = 0;
        _turn = Turn.Idle;
        isSomeonesTurn = false;
        StartNextTurn();
    }
    #endregion

    #region Event-based
    bool isAnimating = false;

    private void OnEnable()
    {
        CombatAnimationListener.AnimationFinished += AnimationEnded;
        CombatMouseListener.MouseClicked += SelectOptions;
    }

    private void OnDisable()
    {
        CombatAnimationListener.AnimationFinished -= AnimationEnded;
        CombatMouseListener.MouseClicked -= SelectOptions;
    }

    void AnimationEnded()
    {
        isAnimating = false;
    }

    IEnumerator BeginAnimation(Animator a, string animationName)
    {
        isAnimating = true;
        a.SetBool(animationName, true);
        while (isAnimating)
        {
            yield return null;
        }
        a.SetBool(animationName, false);
        yield return new WaitForSeconds(.2f);
    }
    #endregion
}