using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;
    private int currentPlayerLevel;
    private Dictionary<string, Quest> questMap;
    private static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.QuestEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.QuestEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.QuestEvents.onQuestStepStateChange += QuestStepStateChange;

        //GameEventsManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.QuestEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.QuestEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.QuestEvents.onQuestStepStateChange -= QuestStepStateChange;

        //GameEventsManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.In_Progress)
                quest.InstantiateCurrentQuestStep(this.transform);
            GameEventsManager.instance.QuestEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.QuestEvents.QuestStateChange(quest);
    }

    private void PlayerLevelChange(int level) => currentPlayerLevel = level;

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        if (currentPlayerLevel < quest.info.levelRequirement)
            meetsRequirements = false;

        foreach(var i in quest.info.dialoguePrerequisites)
            if (!DialogueManager.Instance.dialogueList.CheckIfCompleted(i))
                meetsRequirements = false;

        foreach(var i in quest.info.encounterPrerequisites)
            if (!EncounterList.Instance.GetEncounter(i))
                meetsRequirements = false;

        foreach (QuestSO prerequisiteQuestInfo in quest.info.questPrerequisites)
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.Completed)
                meetsRequirements = false;

        return meetsRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
            if (quest.state == QuestState.Requirements_Not_Met && CheckRequirementsMet(quest))
                ChangeQuestState(quest.info.id, QuestState.Can_Start);
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.In_Progress);
        AnalyticsManager.Instance.SentAnalyticsData(AnalyticsDataEvents.QuestStarted, quest.info.id);
        Debug.LogWarning($"Rozpoczeto quest '{quest.info.displayName}'");
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
            quest.InstantiateCurrentQuestStep(this.transform);
        
        else
            ChangeQuestState(quest.info.id, QuestState.Can_Finish);
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.Completed);
        Debug.LogWarning($"Zakonczono quest '{quest.info.displayName}'");
        AnalyticsManager.Instance.SentAnalyticsData(AnalyticsDataEvents.QuestCompleted, quest.info.id);
        DialogueManager.Instance.CheckRequirements();
    }

    private void ClaimRewards(Quest quest)
    {
        //GameEventsManager.instance.goldEvents.GoldGained(quest.info.goldReward);
        //GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
        Debug.LogWarning("Odbieram nagrody");
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }

        /*
        foreach(KeyValuePair<string, Quest> temp in idToQuestMap)
        {
            Debug.Log($"ID {temp.Key} QUEST STATUS {temp.Value.state}");
        }
        */
        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }
    
    private void OnApplicationQuit() => SaveAllQuests();

    public void SaveAllQuests()
    {
        foreach (Quest quest in questMap.Values)
            SaveQuest(quest);
    }
    
    public void LoadAllQuests() => questMap = CreateQuestMap();

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializedData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.info.id, serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
        }
    }
    
    private Quest LoadQuest(QuestSO questInfo)
    {
        Quest quest = null;
        try
        {
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            else
                quest = new Quest(questInfo);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }

    public QuestState CheckQuestState(string q)
    {
       Quest m = questMap[q];
       return m.state;
    }
}