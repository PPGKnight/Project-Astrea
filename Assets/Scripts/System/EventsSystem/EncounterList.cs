using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterList : MonoBehaviour
{
    static EncounterList instance;
    public static EncounterList Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // false = nie walczono
    // true = walka wygrana
    Dictionary<string, bool> encounterList = new Dictionary<string, bool>{
      {"TavernTutorial1", false},  
      {"TavernTutorial2", false},
      {"002_LumbererRoadblock", false},
      {"002_LumbererEncounter", false}
    };

    public bool GetEncounter(string s)
    {
        if (s.Contains("&"))
        {
            Debug.Log(s);
            s = s.Remove(s.Length-1);
            Debug.Log(s);
            return (encounterList[s + "1"] || encounterList[s + "2"]);
        }
        else
            return encounterList[s];
    }

    public void SetEncounter(string s)
    {
        encounterList[s] = true;
        DialogueManager.Instance.CheckRequirements();
    }
}