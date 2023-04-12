using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BattleState
    {
        None,
        Setup,
        Start,
        Victory,
        Defeat
    }
public class BattleManager : MonoBehaviour
{
    private List<Player> allies;
    private List<Enemy> enemies;
    private List<Creature> queue;

    private BattleState battleState;

    [SerializeField]
    private BattleData battleData;
    [SerializeField]
    private Canvas _battleUI;
    [SerializeField]
    private GameObject _characterPanel;

    private void Awake()
    {
        queue = new List<Creature>();
        Setup();
    }
    public void Setup()
    {
        this.allies = battleData.allies;
        this.enemies = battleData.enemies;
        GameManager.Instance.worldTime = 0;
        battleState = BattleState.Setup;
        SetupArena();
    }

    private void SetupArena()
    {
        if (battleState != BattleState.Setup) return;
        sbyte index = 1;
        foreach (Player player in allies)
        {
            GameObject temp = GameObject.Find("Ally" + index);
            GameObject p = Instantiate(GameManager.Instance.entity[player.Name], temp.transform);
            p.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            p.transform.transform.localScale = new Vector3(1f, 2f, 1f);
            p.transform.parent = temp.transform;
            queue.Add(player);

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
            GameObject e = Instantiate(GameManager.Instance.entity[enemy.Name], temp.transform);
            e.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            e.transform.parent = temp.transform;
            queue.Add(enemy);
            index++;
        }
        
    }



    
}
