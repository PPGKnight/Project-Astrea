using UnityEngine;
using System.Collections.Generic;

public enum BattleState
    {
        None,
        Setup,
        Start,
        Victory,
        Defeat
    }
public class BattleManager : ScriptableObject
{
    private BattleState battleState;

    public List<Player> allies = new List<Player>();
    public List<GameObject> enemies = new List<GameObject>();
    private List<Creature> queue = new List<Creature>();

    public void Battle()
    {
        GameManager.Instance.worldTime = 0;
        battleState = BattleState.Setup;
        Debug.Log("Walka wczytana pomyœlnie");
        SetupArena();
    }

    private void SetupArena()
    {
        if (battleState != BattleState.Setup) return;
        sbyte index = 1;
        foreach(Player player in allies)
        {
            GameObject temp = GameObject.Find("Ally"+index);
            //Instantiate(player, temp.transform);
            queue.Add(player);
            index++;
        }
        index = 1;
        foreach(GameObject enemy in enemies)
        {
            GameObject temp = GameObject.Find("Enemy" + index);
            Debug.Log(temp);
            //Instantiate(enemy, new Vector3(-8f - (index * 3), 101f, 8f - (index * 3)), Quaternion.identity);
            //var e = Instantiate(enemy, new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            var e = Instantiate(enemy, temp.transform);
            e.transform.transform.localPosition = new Vector3(0f, 2f, 0f);
            //e.transform.parent = temp.transform;
            //e.transform.position.y = 0f;
            queue.Add(enemy.GetComponent<Enemy>());
            index++;
        }
        queue.Sort((x,y) => (y.Initiative).CompareTo(x.Initiative));
        foreach(var q in queue)
            Debug.Log(q.Name + ", " + q.Initiative);
    }

    
}
