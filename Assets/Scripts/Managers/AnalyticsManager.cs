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
        if (!PlayerPrefs.HasKey("analyticsConsent")) return;
        if (PlayerPrefs.GetInt("analyticsConsent") == 0) return;
        AnalyticsService.Instance.StartDataCollection();
    }

    Dictionary<AnalyticsDataEvents, string> dataEvents = new Dictionary<AnalyticsDataEvents, string>()
    {
        {AnalyticsDataEvents.Consent, "ConsentToUseAnalytics"},
        {AnalyticsDataEvents.FirstDialogue, "BakerVsCabbageMan"},
        {AnalyticsDataEvents.FirstQuestline, "NewJourney"}
    };

    public void SentAnalyticsData(AnalyticsDataEvents eventName, object value)
    {
        if (PlayerPrefs.GetInt("analyticsConsent") == 0) return;

        try
        {
            AnalyticsService.Instance.CustomData(eventName.ToString(), new Dictionary<string, object> { { dataEvents[eventName], value } });
            Debug.Log($"The data has been successfully sent");
        }
        catch (Exception)
        {
            Debug.LogError($"Sending analytics data failed for event {eventName} with value {value}");
        }
    }

    public void StartCollecting()
    {
        PlayerPrefs.SetInt("analyticsConsent", 1);
        AnalyticsService.Instance.StartDataCollection();
    }
    public void DeleteData() => AnalyticsService.Instance.RequestDataDeletion();
    public void StopCollecting()
    {
        PlayerPrefs.SetInt("analyticsConsent", 0);
        AnalyticsService.Instance.StopDataCollection();
    }
}

public enum AnalyticsDataEvents
{
    Consent,
    FirstQuestline,
    FirstDialogue
}