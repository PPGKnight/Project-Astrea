using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    GameObject questInfoPrefab;

    [SerializeField]
    Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.onStartQuest += AddQuest;
        GameEventsManager.instance.QuestEvents.onQuestProgress += ProgressQuest;
        GameEventsManager.instance.QuestEvents.onFinishQuest += RemoveQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.onStartQuest -= AddQuest;
        GameEventsManager.instance.QuestEvents.onQuestProgress -= ProgressQuest;
        GameEventsManager.instance.QuestEvents.onFinishQuest -= RemoveQuest;
    }

    private void Start()
    {
        activeQuests.Clear();
    }

    void AddQuest(string id)
    {
        if (!activeQuests.ContainsKey(id))
        {
            GameObject temp = Instantiate(questInfoPrefab, this.transform);
            Vector3 vec = new Vector3(0f, 75f * activeQuests.Count, 0f);
            temp.GetComponent<RectTransform>().anchoredPosition = vec;
            activeQuests.Add(id, temp);
        }
    }

    void ProgressQuest(string id, IQuestProgress iqp)
    {
        if(activeQuests.ContainsKey(id))
        {
            TextMeshProUGUI[] qText = activeQuests[id].GetComponentsInChildren<TextMeshProUGUI>();
            Quest q = QuestManager.Instance.GetQuestById(id);
            qText[0].text = q.info.displayName;
            qText[1].text = iqp.QuestProgress();
        }    
    }

    void RemoveQuest(string id)
    {
        if (activeQuests.ContainsKey(id))
        {
            Destroy(activeQuests[id]);
            activeQuests.Remove(id);
        }
    }
}
