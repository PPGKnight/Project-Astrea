using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class Analytics : MonoBehaviour
{
    [SerializeField]
    GameObject consentBox;
    bool userConsent;

    [SerializeField]
    bool debugRemovePrefs;

    async void Start()
    {
        if(debugRemovePrefs)
            PlayerPrefs.DeleteKey("analyticsConsent");

        consentBox.SetActive(false);
        await UnityServices.InitializeAsync();

        if (!PlayerPrefs.HasKey("analyticsConsent"))
            ConsentBox();
        else
        {
            userConsent = PlayerPrefs.GetInt("analyticsConsent") == 1 ? true : false;
            if (userConsent) Debug.Log("The user has already consented to data collection. No additional consent is required");
            else Debug.Log("The user has not consented to data collection. No additional consent is required");      
        }
    }

    Dictionary<string, object> test = new Dictionary<string, object>(){
         {"working", true},
         {"happy", false}
     };

    Dictionary<string, object> consent = new Dictionary<string, object>()
    {
        {"ConsentToUseAnalytics", true}
    };

    void BeginAnalytics()
    {
        AnalyticsService.Instance.StartDataCollection();
        AnalyticsService.Instance.CustomData("Consent", consent);
        AnalyticsService.Instance.CustomData("testEvent", test);
        Debug.Log("Data Sent");
    }

    public void ConsentBox() => consentBox.SetActive(true);

    public void Agree()
    {
        Debug.LogError("User has agreed to allow data collection");
        PlayerPrefs.SetInt("analyticsConsent", 1);
        consentBox.SetActive(false);
        BeginAnalytics();
    }
    public void Decline()
    {
        Debug.LogError("User has declined to allow data collection");
        PlayerPrefs.SetInt("analyticsConsent", 0);
        consentBox.SetActive(false);
    }
}