using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;

    // quest start requirements
    private int currentPlayerLevel;

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
        Debug.Log("Rozpoczynam QuestManagera");
        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.state == QuestState.In_Progress)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.instance.QuestEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.QuestEvents.QuestStateChange(quest);
    }

    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        // check player level requirements
        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

        // check dialogue prerequisities
        foreach(var i in quest.info.dialoguePrerequisites)
        {
            if (!DialogueManager.Instance.dialogueList.CheckIfCompleted(i))
                meetsRequirements = false;
        }

        // check encounter prerequisities
        foreach(var i in quest.info.encounterPrerequisites)
        {
            if (!EncounterList.Instance.GetEncounter(i))
                meetsRequirements = false;
        }

        // check quest prerequisites for completion
        foreach (QuestSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.Completed)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        // loop through ALL quests
        foreach (Quest quest in questMap.Values)
        {
            // if we're now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.Requirements_Not_Met && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.Can_Start);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.In_Progress);
        Debug.LogWarning($"Rozpoczeto quest '{quest.info.displayName}'");
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        // move on to the next step
        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        // if there are no more steps, then we've finished all of them for this quest
        else
        {
            ChangeQuestState(quest.info.id, QuestState.Can_Finish);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.Completed);
        Debug.LogWarning($"Zakonczono quest '{quest.info.displayName}'");
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
        // loads all QuestSO Scriptable Objects under the Assets/Resources/Quests folder
        QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");
        // Create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }

        foreach(KeyValuePair<string, Quest> temp in idToQuestMap)
        {
            Debug.Log($"ID {temp.Key} QUEST STATUS {temp.Value.state}");
        }
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
    
    private void OnApplicationQuit()
    {
        SaveAllQuests();
    }

    public void SaveAllQuests()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }
    
    public void LoadAllQuests()
    {
        questMap = CreateQuestMap();
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            // serialize using JsonUtility, but use whatever you want here (like JSON.NET)
            string serializedData = JsonUtility.ToJson(questData);
            // saving to PlayerPrefs is just a quick example for this tutorial video,
            // you probably don't want to save this info there long-term.
            // instead, use an actual Save & Load system and write to a file, the cloud, etc..
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
            // load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // otherwise, initialize a new quest
            else
            {
                quest = new Quest(questInfo);
            }
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
       Debug.Log($"Quest {q} has state {m.state}");
        return m.state;
    }
}