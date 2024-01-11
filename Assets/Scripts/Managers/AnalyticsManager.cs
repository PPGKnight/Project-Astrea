using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    static AnalyticsManager instance;
    public static AnalyticsManager Instance { get { return instance; } }

    async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        await UnityServices.InitializeAsync();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("analyticsConsent", 0);
        if (!PlayerPrefs.HasKey("analyticsConsent")) return;
        if (PlayerPrefs.GetInt("analyticsConsent") == 0) return;
        AnalyticsService.Instance.StartDataCollection();
    }

    Dictionary<AnalyticsDataEvents, string> event_to_param = new Dictionary<AnalyticsDataEvents, string>(){
        { AnalyticsDataEvents.Consent, "ConsentToUseAnalytics"},
        { AnalyticsDataEvents.QuestCompleted, "QuestCompleted"},
        { AnalyticsDataEvents.DialogueOptionChosen, "DialogueOptionChosen"},
    };

    public void SentAnalyticsData(AnalyticsDataEvents eventToSend, object value)
    {
        if (PlayerPrefs.GetInt("analyticsConsent") == 0) return;

        try
        {
            AnalyticsService.Instance.CustomData(eventToSend.ToString(), new Dictionary<string, object> { { event_to_param[eventToSend], value } });
            Debug.Log($"The data has been successfully sent");
        }
        catch (Exception)
        {
            Debug.LogError($"Sending analytics data failed for event {event_to_param[eventToSend]} with value {value}");
        }
    }

    public void StartCollecting()
    {
        PlayerPrefs.SetInt("analyticsConsent", 1);
        AnalyticsService.Instance.StartDataCollection();
        PlayerPrefs.Save();
    }
    public void DeleteData() => AnalyticsService.Instance.RequestDataDeletion();
    public void StopCollecting()
    {
        PlayerPrefs.SetInt("analyticsConsent", 0);
        AnalyticsService.Instance.StopDataCollection();
        PlayerPrefs.Save();
    }
}

public enum AnalyticsDataEvents
{
    Consent,
    QuestCompleted,
    DialogueOptionChosen
}