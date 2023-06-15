using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class QuestSystem : MonoBehaviour
{
    public List<Quest> quests;

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    public void CheckQuestProgress()
    {
        foreach(Quest quest in quests)
        {
            bool isQuestCompleted = true;

            foreach(Task task in quest.tasks)
            {
                if(!task.isCompleted)
                {
                    isQuestCompleted = false;
                    break;
                }
            }

            if(isQuestCompleted)
            {
                //GrantReward(quest.reward);
            }
        }
    }
/*    public void GrantReward()
    {
        PlayerStats.AddExperience(reward.experiencePoints);

        foreach (Item item in reward.items)
        {
            Inventory.AddItem(item);
        }
    }*/

}

public class Quest
{
    public string questName;
    public string questDescription;
    public List<Task> tasks;
    public Reward reward;

    public Quest(string name, string description, List<Task> tasklist, Reward questReward)
    {
        questName = name;
        questDescription = description;
        tasks = tasklist;
        reward = questReward;
    }
}

public class Task
{
    public string taskDescription;
    public bool isCompleted;

    public Task(string description)
    {
        taskDescription = description;
        isCompleted = false;
    }
}

public class Reward
{
    public int experiencePoints;
    public List<Item> items;

    public Reward(int expPoints, List<Item> itemList)
    {
        experiencePoints = expPoints;
        items = itemList;
    }
}
