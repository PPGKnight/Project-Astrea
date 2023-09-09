using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/QuestSO", order = 1)]
public class QuestSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestSO[] questPrerequisites;
    public string[] dialoguePrerequisites;
    public string[] encounterPrerequisites;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;

    private void OnValidate()
    {
    #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    #endif
    }
}
