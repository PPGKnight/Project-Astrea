using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    public string encounterID;
    public Enemy[] enemies;
    public string arenaName;

    [SerializeField]
    private EncounterType encounterType;

    private void Start()
    {
        GameManager.Instance.Check();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (encounterType != EncounterType.WhenInRange) return;

        if (other.gameObject.CompareTag("MainPlayer"))
        {
            GameManager.Instance.BattleStart(enemies, encounterID, arenaName);
        }
    }
}
enum EncounterType
{
    WhenInRange,
    Dialogue
}