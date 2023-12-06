using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    [SerializeField]
    GameObject consentBox;
    bool userConsent;

    [SerializeField]
    bool debugRemovePrefs;

    async void Awake() => await UnityServices.InitializeAsync();

    void Start()
    {
        if(debugRemovePrefs)
            PlayerPrefs.DeleteKey("analyticsConsent");

        consentBox.SetActive(false);

        if (!PlayerPrefs.HasKey("analyticsConsent"))
            ConsentBox();
        else
        {
            userConsent = PlayerPrefs.GetInt("analyticsConsent") == 1 ? true : false;
            if (userConsent) Debug.Log("The user has already consented to data collection. No additional consent is required");
            else Debug.Log("The user has not consented to data collection. No additional consent is required");      
        }
    }

    void BeginAnalytics()
    {
        AnalyticsService.Instance.StartDataCollection();
        AnalyticsService.Instance.CustomData(AnalyticsDataEvents.Consent.ToString(), new Dictionary<string, object>{
            { "ConsentToUseAnalytics", true } 
        });
        Debug.Log("Data Sent");
    }

    public void ConsentBox() => consentBox.SetActive(true);

    public void Agree()
    {
        Debug.Log("User has agreed to allow data collection");
        PlayerPrefs.SetInt("analyticsConsent", 1);
        consentBox.SetActive(false);
        BeginAnalytics();
    }
    public void Decline()
    {
        Debug.Log("User has declined to allow data collection");
        PlayerPrefs.SetInt("analyticsConsent", 0);
        consentBox.SetActive(false);
    }
}