using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    public int encounterID;
    public Enemy[] enemies;

    [SerializeField]
    private EncounterType encounterType;

    private void OnTriggerEnter(Collider other)
    {
        if (encounterType != EncounterType.WhenInRange) return;

        if (other.gameObject.CompareTag("MainPlayer"))
        {
            GameManager.Instance.BattleStart(enemies);
        }
    }
}
enum EncounterType
{
    WhenInRange,
    Dialogue
}