using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
    [SerializeField] public Animator animator;

    private BattleState _battleState;
    private Turn _turn;
    private TurnOptions _turnOptions;

    public FloatingNumbers _floatingNumbers;

    [SerializeField]
    private BattleData battleData;
    [SerializeField]
    private Canvas _battleUI;
    [SerializeField]
    private GameObject _characterPanel;
    [SerializeField]
    private GameObject _enemyPanel;
    [SerializeField]
    private Canvas _battleOptions;
    //private bool isSetup = false;

    //private event Action<Creature> _whoseTurn;
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
        //_whoseTurn += DoTurn;
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

            #region AllyUI
            GameObject p_characterPanel = Instantiate(_characterPanel, _battleUI.transform);
            p_characterPanel.transform.SetParent(_battleUI.transform);
            
            p_characterPanel.transform.localPosition = new Vector3(1075f - (index*325f), -420f, 0f);
            p.entityInfo = p_characterPanel.GetComponent<UpdateInfo>();
            p.entityInfo.SetInfo(p.Name, p.Level, p.CurrentHP, p.MaxHP, p.CurrentMana, p.MaxMana);
            #endregion
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
        Debug.Log("Jeszcze �yj� #1");
        //Fight();
        _battleState = BattleState.Battle;
        //isSetup = true;
    }

    private string action = "";
    private GameObject target = null;
    private bool isSomeonesTurn = false;
    int alliesDeaths = 0;
    int enemiesDeaths = 0;
    #endregion

    #region WorkingWithoutDelay
    void Fight()
    {
        //Debug.Log(isSomeonesTurn);
        if (isSomeonesTurn) return;

        foreach(Creature c in queue)
        {
            animator = GetComponent<Animator>();
            c.tracker += Mathf.RoundToInt(((c.Dexterity + c.Strength) * 0.25f + c.SpeedBonus)); 
            //Debug.Log($"{c.Name} posiada {c.tracker} / 100 inicjatywy!");
            //bool a = (c.tracker >= 100);
            //Debug.Log($" Wi�cej od 100? {c.tracker} {a}");
            if(c.tracker >= 100f)
            {
                if (c.HadTurn) continue;
                //c.tracker = 0;
                Debug.Log($"Tura: {c.Name}");
                isSomeonesTurn = true;
                if (c.GetCreatureType() == "Enemy") { _turn = Turn.Enemy; DoEnemyTurn(c); }
                else _turn = Turn.Ally;
                //_whoseTurn?.Invoke(c);

            }
        }
        foreach (Creature c in queue) c.HadTurn = false;
    }

    public void SetAction(string s)
    {
        action = s;
        Debug.Log($"Ustawiono akcj� na {action}");
        _turnOptions = TurnOptions.Target;
    }

    public void CheckDeaths()
    {
        for(int t = queue.Count - 1; t >= 0; t--)
        {        
            Creature c = queue[t];
            if (c.IsDead()) { 
                Debug.Log($"{c.Name} zosta� zg�adzony!");
                if (c.GetCreatureType() == "Enemy") enemiesDeaths++;
                else alliesDeaths++;

                queue.Remove(c);
                //c.gameObject.SetActive(false);
                Destroy(c.entityInfo.gameObject);
                Destroy(c.gameObject);
                //Debug.Log($"{alliesDeaths} + {enemiesDeaths}");
            }
            
            if (enemiesDeaths == enemies.Count) {
                Debug.Log("Uda�o Ci si� wygra� bitw�!");
                _battleState = BattleState.Victory;
                GameManager.Instance.ReturnToScene();
            }
            else if (alliesDeaths == allies.Count)
            {
                Debug.Log("Niestety przegra�e� t� walk�!");
                _battleState = BattleState.Defeat;
                GameManager.Instance.ReturnToScene();
            }
            //Task.Delay(2000);
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
                    if (action.Length > 0 && target != null && _turn == Turn.Ally) ManualCheck();
                }
            }
            else _battleOptions.gameObject.SetActive(false);
        }
    }

    void ManualCheck()
    {
        Creature c = queue.Where(t => t.tracker >= 100f).ToList()[0];
        DoTurn(c);
        //foreach(Creature c in queue)
        //{
        //    if (c.tracker >= 100f)
        //        if (c.GetCreatureType() == "Ally")
        //            DoTurn(c);
        //}
    }

    void DoTurn(Creature c)
    {
        if (action.Length > 0 && target != null)
        {
            //Debug.Log($"Jestem w DoTurn z obiektem {c.Name}!");
            FloatingNumbers f;
            switch (action)
            {
                case "Attack":
                    print($"You attacked {target.GetComponent<Enemy>().Name} for {c.GetComponent<Player>().Attack()} damage!");
                    c.GetComponent<Animator>().SetBool("isAttacking", true);
                    target.GetComponent<Enemy>().TakeDamage(c.GetComponent<Player>().Attack());
                    target.GetComponent<Animator>().SetBool("isReacting",true);
                    f = Instantiate(_floatingNumbers, target.transform.position, Quaternion.identity);
                    f.SetText(c.Attack(), Color.red);
                    target.GetComponent<Enemy>().entityInfo.UpdateHP(target.GetComponent<Enemy>().CurrentHP);
                    break;
                case "Heal":
                    print($"You heal {target.GetComponent<Player>().Name} for {c.GetComponent<Player>().Intelligence} health points!");
                    c.GetComponent<Animator>().SetBool("isHealing", true);
                    target.GetComponent<Player>().Heal(c.GetComponent<Player>().Intelligence);
                    f = Instantiate(_floatingNumbers, target.transform.position, Quaternion.identity);
                    f.SetText(c.Intelligence, Color.green);
                    target.GetComponent<Player>().entityInfo.UpdateHP(target.GetComponent<Player>().CurrentHP);
                    break;
                case "Guard":
                    print($"You will take -50% damage on enemy's next attack");
                    c.GetComponent<Animator>().SetBool("isGuarding", true);
                    c.GetComponent<Player>().Guard();
                    break;
            }

            target = null;
            action = "";
            _turn = Turn.Idle;
            _turnOptions = TurnOptions.Idle;
            c.tracker = 0;
            isSomeonesTurn = false;
          //  c.GetComponent<Animator>().SetBool("isAttacking", false);
          //  c.GetComponent<Animator>().SetBool("isHealing", false);
          //  c.GetComponent<Animator>().SetBool("isGuarding", false);
        }
    }

    void DoEnemyTurn(Creature c)
    {
        _turnOptions = TurnOptions.Idle;

        List<Creature> insideAllies = queue.Where(t => t.GetCreatureType() == "Ally").ToList();
        List<Creature> insideEnemies = queue.Where(t => t.GetCreatureType() == "Enemy").ToList();

        System.Random rnd = new System.Random();
        FloatingNumbers f;
        switch (rnd.Next(0, 3))
        {
            case 0:
                int att = rnd.Next(0, insideAllies.Count);
                Debug.Log($"{c.Name} atakuje {insideAllies[att].Name} za {c.Attack()} punkt�w obra�e�!");
                c.GetComponent<Animator>().SetBool("isAttacking", true);
                insideAllies[att].TakeDamage(c.Attack());
                f = Instantiate(_floatingNumbers, insideAllies[att].transform.position, Quaternion.identity);
                f.SetText(c.Attack(), Color.red);
                insideAllies[att].entityInfo.UpdateHP(insideAllies[att].CurrentHP);
                break;
            case 1:
                insideEnemies.Sort((a, b) => a.CurrentHP.CompareTo(b.CurrentHP));
                Debug.Log($"{c.Name} leczy {insideEnemies[0].Name} za {c.Intelligence} punkt�w zdrowia");
                c.GetComponent<Animator>().SetBool("isHealing", true);
                insideEnemies[0].Heal(c.Intelligence);
                f = Instantiate(_floatingNumbers, insideAllies[0].transform.position, Quaternion.identity);
                f.SetText(c.Intelligence, Color.green);
                insideEnemies[0].entityInfo.UpdateHP(insideEnemies[0].CurrentHP);
                break;
            case 2:
                int bl = rnd.Next(0, insideEnemies.Count);
                Debug.Log($"{c.Name} broni {insideEnemies[bl].Name} przez co ten otrzyma o po�ow� mniej obra�e�");
                c.GetComponent<Animator>().SetBool("isGuarding", true);
                insideEnemies[bl].Guard();
                break;
        }

        c.tracker = 0;
        _turn = Turn.Idle;
        isSomeonesTurn = false;
        // c.GetComponent<Animator>().SetBool("isAttacking", false);
        // c.GetComponent<Animator>().SetBool("isHealing", false);
        // c.GetComponent<Animator>().SetBool("isGuarding", false);
    }

    IEnumerator FloatingNumbers()
    {

        yield return null;
    }


    #endregion
}