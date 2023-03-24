using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    public Enemy[] enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainPlayer"))
            GameManager.Instance.BattleStart(enemies);
    }
}
