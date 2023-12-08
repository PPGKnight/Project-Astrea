using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    static BattleManager _instance;
    public static BattleManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] Camera m_camera;
    [SerializeField] BattleData battleData;
    [SerializeField] Canvas _battleUI;
    [SerializeField] GameObject _characterPanel;
    [SerializeField] GameObject _enemyPanel;
    [SerializeField] Canvas _battleOptions;
    
    List<Player> allies;
    List<Enemy> enemies;
    List<Creature> queue;
    Animator animator;
    Creature activeCreature;

    BattleState _battleState;
    Turn _turn;
    TurnOptions _turnOptions;
    TurnManager turnManager;

    public FloatingNumbers _floatingNumbers;
    public InitiativeTrackerManager initiativeTrackerManager;
    bool isAnimating = false;
    int loopIndex = 1;
    private string action = "";
    private GameObject target = null;
    private bool isSomeonesTurn = false;
    public float DebugMoveWaitForSeconds = 1.75f;

    public static event Action ProgressTurn;
    public static event Action RemoveTokens;
    public static event Action UpdateBars;
    public static event Action<Creature, int> TakeDamage;
    public static event Action<Creature, int> HealDamage;
    public static event Action<Creature> Guard;
    public static event Action AdvanceLoop;

    private void OnEnable()
    {
        CombatAnimationListener.AnimationFinished += AnimationEnded;
        CombatMouseListener.MouseClicked += SelectOptions;
        TakeDamage += DoTheDamage;
        HealDamage += HealTheDamage;
        Guard += GuardFromDamage;
        AdvanceLoop += Advance;
    }

    private void OnDisable()
    {
        CombatAnimationListener.AnimationFinished -= AnimationEnded;
        CombatMouseListener.MouseClicked -= SelectOptions;
        TakeDamage -= DoTheDamage;
        HealDamage -= HealTheDamage;
        Guard -= GuardFromDamage;
        AdvanceLoop -= Advance;
    }

    #region ArenaSetup
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

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
            p.transform.transform.localPosition = new Vector3(0f, 1f, 0f);
            p.transform.transform.localScale = new Vector3(1f, 1f, 1f);
            p.transform.rotation = temp.transform.rotation;
            queue.Add(p);
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
            e.transform.transform.localPosition = new Vector3(0f, 0f, 0f);
            e.transform.rotation = temp.transform.rotation;
            queue.Add(e);
            index++;
        }

        foreach (Creature q in queue)
            q.SetInitiative();

        turnManager = new TurnManager(queue);

        CreateTokens();
        _battleState = BattleState.Battle;
    }

    private void Start() => AdvanceLoop.Invoke();

    void CreateTokens() => initiativeTrackerManager.CreateTokens();


    #endregion

    #region UpdateLoop
    private void Update()
    {
        if (isSomeonesTurn && _turn == Turn.Ally && action.Length <= 0) _turnOptions = TurnOptions.Options;
        
        if ((_turnOptions == TurnOptions.Options || _turnOptions == TurnOptions.Target))
                _battleOptions.gameObject.SetActive(true);
        else _battleOptions.gameObject.SetActive(false);
        
    }
    #endregion

    #region AnimationCoroutines

    IEnumerator BeginAnimation(Animator a, string animationName)
    {
        isAnimating = true;
        Quaternion originRot = a.gameObject.transform.rotation;
        a.SetBool(animationName, true);
        while (isAnimating)
        {
            yield return null;
        }
        a.SetBool(animationName, false);
        a.gameObject.transform.rotation = originRot;
        yield return new WaitForSeconds(.2f);
    }

    //IEnumerator BeginAnimationB(Animator a, string animationName, Transform attacker, Transform attackPosition)
    //{
    //    Vector3 original = attacker.position;
    //    isAnimating = true;
    //    a.SetBool(animationName, true);
    //    attacker.DOMove(attackPosition.position, 0.5f);
    //    while (isAnimating)
    //    {
    //        yield return null;
    //    }
    //    attacker.DOMove(original, 0.5f);
    //    Vector3 resetRot = Vector3.zero;
    //    attacker.rotation = Quaternion.Euler(resetRot);
    //    a.SetBool(animationName, false);
    //    yield return new WaitForSeconds(.2f);
    //}

    IEnumerator BeginAnimationB(Animator a, string animationName, Transform attacker, Transform attackPosition, Animator b, string animationNameB)
    {
        isAnimating = true;
        StartCoroutine(AnimationAttack(a, animationName, attacker, attackPosition));
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(AnimationReact(b, animationNameB));
        while (isAnimating)
        {
            yield return null;
        }
        //a.SetBool(animationName, false);
        //b.SetBool(animationNameB, false);
        yield return new WaitForSeconds(.2f);
    }

    IEnumerator AnimationAttack(Animator a, string animName, Transform attacker, Transform pos)
    {
        Quaternion originRot = attacker.rotation;
        Debug.LogWarning($"OROT: {originRot}, Rot: {attacker.rotation}");
        Vector3 original = attacker.position;
        //a.SetBool(animName, true);
        a.Play("Running");
        attacker.DOMove(pos.position, 0.5f);
        yield return new WaitForSeconds(.3f);
        a.Play("Hook Punch");
        yield return new WaitForSeconds(2f);
        a.Play("Running Backwards");
        attacker.DOMove(original, 0.5f);
        attacker.DORotate(Vector3.zero, .1f).OnComplete(() => {
            attacker.rotation = originRot;
        });
        Vector3 resetRot = Vector3.zero;
        Debug.LogWarning($"OROT2: {originRot}");
        //attacker.rotation = originRot;
        Debug.LogWarning($"Rot: {attacker.rotation}");
        yield return null;
    }

    IEnumerator AnimationReact(Animator b, string animName)
    {
        //b.SetBool(animName, true);
        b.Play("Reaction");
        yield return null;
    }
    #endregion

    #region CombatLoop
    void CombatLoop()
    {
        loopIndex = loopIndex > 5 ? loopIndex = 1 : loopIndex;
        Debug.Log(loopIndex);
        switch (loopIndex)
        {
            case 1:
                loopIndex++;
                // Check if all dead
                //Debug.Log("CombatLoop1");
                Tuple<bool, int> result = turnManager.CheckResults();
                if (result.Item1) FinalizeFight(result);
                else AdvanceLoop.Invoke();
                break;
            case 2:
                loopIndex++;
                // Get next creature
                //Debug.Log("CombatLoop2");
                activeCreature = turnManager.GetCreature();
                Debug.LogError(activeCreature.Name);
                AdvanceLoop.Invoke();
                break;
            case 3:
                loopIndex++;
                // Do its turn
                //Debug.Log("CombatLoop3");
                if(activeCreature.GetCreatureType() == "Enemy")
                    DoEnemyTurn(activeCreature);
                else
                    AllyTurn();
                break;
            case 4:
                loopIndex++;
                // Check Deaths
                //Debug.Log("CombatLoop4");
                turnManager.CheckDeaths();
                AdvanceLoop.Invoke();
                break;
            case 5:
                // Shift order
                //Debug.Log("CombatLoop5");
                loopIndex++;
                turnManager.NextTurn();
                AdvanceLoop.Invoke();
                ProgressTurn?.Invoke();
                break;
        }
    }

    void Advance()
    {
        Debug.Log("Advance called");
        CombatLoop();
    }
    public void SetAction(string s)
    {
        action = s;
        Debug.Log($"Ustawiono akcje na {action}");
        _turnOptions = TurnOptions.Target;
    }

    void FinalizeFight(Tuple<bool, int> result)
    {
        switch(result.Item2)
        {
            case 1:
                _battleState = BattleState.Defeat;
                battleData.battleStatus = BattleStatus.Defeat;
                break;

            case 2:
                _battleState = BattleState.Victory;
                battleData.battleStatus = BattleStatus.Victory;
                break;
        }
        StartCoroutine(LeaveBattle());
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
                if ((hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("MainPlayer")) && !hit.transform.GetComponent<Creature>().IsDead())
                {
                    if (hit.transform.CompareTag("Enemy"))
                        Debug.Log($"Trafiono {hit.transform.GetComponent<Enemy>().Name}");
                    if (hit.transform.CompareTag("MainPlayer") || hit.transform.CompareTag("Ally"))
                        Debug.Log($"Trafiono {hit.transform.GetComponent<Player>().Name}");

                    target = hit.transform.gameObject;
                    StartCoroutine(DoTurn(activeCreature));
                    _turnOptions = TurnOptions.Idle;
                }
            }
        }
    }
    #endregion

    #region AllyTurn
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
            Coroutine coro;
            switch (action)
            {
                case "Attack":
                    int a = c.GetComponent<Player>().Attack();
                    Transform t = GameObject.Find(target.gameObject.transform.parent.name + "Hit").transform;
                    yield return new WaitForSeconds(0.1f);
                    //yield return coro = StartCoroutine(BeginAnimationB(c.GetComponent<Animator>(), "isAttacking", c.transform, t));
                    //StopCoroutine(coro);
                    //yield return coro = StartCoroutine(BeginAnimation(target.GetComponent<Animator>(), "isReacting"));
                    yield return coro = StartCoroutine(BeginAnimationB(c.GetComponent<Animator>(), "isAttacking", c.transform, t, target.GetComponent<Animator>(), "isReacting"));
                    StopCoroutine(coro);
                    print($"You attacked {target.GetComponent<Enemy>().Name} for {a} damage!");
                    target.GetComponent<Enemy>().TakeDamage(a);
                    TakeDamage.Invoke(target.GetComponent<Creature>(), a);
                    target.GetComponent<Enemy>().entityInfo.UpdateHP(target.GetComponent<Enemy>().CurrentHP);
                    break;
                case "Heal":
                    print($"You heal {target.GetComponent<Player>().Name} for {c.GetComponent<Player>().Intelligence} health points!");
                    yield return coro = StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isHealing"));
                    StopCoroutine(coro);
                    int healingAmount = c.GetComponent<Player>().Intelligence;
                    target.GetComponent<Player>().Heal(healingAmount);
                    HealDamage.Invoke(target.GetComponent<Creature>(), healingAmount);
                    break;
                case "Guard":
                    print($"You will take -50% damage on enemy's next attack");
                    yield return coro = StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isGuarding"));
                    StopCoroutine(coro);
                    c.GetComponent<Player>().Guard();
                    Guard.Invoke(target.GetComponent<Creature>());
                    break;
            }
            target = null;
            action = "";
            _turn = Turn.Idle;
            c.tracker = 0;
            isSomeonesTurn = false;
            AdvanceLoop.Invoke();
        }
    }
    #endregion

    #region EnemyTurn
    void DoEnemyTurn(Creature c)
    {
        if (!isSomeonesTurn)
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
        Coroutine coro;

        switch (rnd.Next(0, 10))
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                int att = rnd.Next(0, insideAllies.Count);
                int a = c.Attack();
                Debug.Log($"Damage c: {c.Attack()}");
                Debug.Log($"Damage active: {activeCreature.Attack()}");
                Transform t = GameObject.Find(insideAllies[att].gameObject.transform.parent.name + "Hit").transform;
                yield return new WaitForSeconds(0.1f);
                //yield return coro = StartCoroutine(BeginAnimationB(c.GetComponent<Animator>(), "isAttacking", c.transform, t));
                //StopCoroutine(coro);
                //yield return coro = StartCoroutine(BeginAnimation(insideAllies[att].GetComponent<Animator>(), "isReacting"));
                yield return coro = StartCoroutine(BeginAnimationB(c.GetComponent<Animator>(), "isAttacking", c.transform, t, insideAllies[att].GetComponent<Animator>(), "isReacting"));
                StopCoroutine(coro);
                Debug.Log($"{c.Name} atakuje {insideAllies[att].Name} za {a} punktow obrazen!");
                insideAllies[att].TakeDamage(a);
                TakeDamage.Invoke(insideAllies[att], a);
                break;
            case 7:
            case 8:
                insideEnemies.Sort((a, b) => a.CurrentHP.CompareTo(b.CurrentHP));
                Debug.Log($"{c.Name} leczy {insideEnemies[0].Name} za {c.Intelligence} punktow zdrowia");
                yield return coro = StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isHealing"));
                StopCoroutine(coro);
                insideEnemies[0].Heal(c.Intelligence);
                HealDamage.Invoke(insideEnemies[0], c.Intelligence);
                insideEnemies[0].GetComponent<Enemy>().entityInfo.UpdateHP(insideEnemies[0].GetComponent<Enemy>().CurrentHP);
                break;
            case 9:
                yield return coro = StartCoroutine(BeginAnimation(c.GetComponent<Animator>(), "isGuarding"));
                StopCoroutine(coro);
                Debug.Log($"{c.Name} broni sie przez co ten otrzyma o polowe mniej obrazen");
                Guard.Invoke(insideEnemies[0]);
                c.Guard();
                break;
        }
        c.tracker = 0;
        _turn = Turn.Idle;
        isSomeonesTurn = false;
        AdvanceLoop.Invoke();
    }
    #endregion

    void AnimationEnded() => isAnimating = false;

    void DoTheDamage(Creature attacked, int damage)
    {
        FloatingNumbers f;
        f = Instantiate(_floatingNumbers, attacked.transform.position, Quaternion.identity);
        f.SetText(damage, Color.red);

        UpdateBars?.Invoke();
    }

    void HealTheDamage(Creature healed, int healingAmount)
    {
        FloatingNumbers f;
        f = Instantiate(_floatingNumbers, healed.transform.position, Quaternion.identity);
        f.SetText(healingAmount, Color.green);
        UpdateBars?.Invoke();
    }

    void GuardFromDamage(Creature guard)
    {
        FloatingNumbers f;
        f = Instantiate(_floatingNumbers, guard.transform.position, Quaternion.identity);
        f.SetText("Block", Color.blue);
    }
    public List<Creature> RequestTrackerOrder() => turnManager.GetTracker();

    public void RemoveDead() => RemoveTokens?.Invoke();
    IEnumerator LeaveBattle()
    {
        yield return StartCoroutine(SmallPause());
        StopAllCoroutines();
        GameManager.Instance.ReturnToScene();
    }

    IEnumerator SmallPause()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Done");
    }
}