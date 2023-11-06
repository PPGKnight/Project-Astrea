// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Services.Core;
// using Unity.Services.Analytics;

// public class EventTest : MonoBehaviour
// {
//     // Start is called before the first frame update
//     async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         Agreed();
//     }

//     Dictionary<string, object> test = new Dictionary<string, object>(){
//         {"working", true},
//         {"happy", false}
//     };

//     // Update is called once per frame
//     void Agreed()
//     {
//         AnalyticsService.Instance.StartDataCollection();
//         AnalyticsService.Instance.CustomData("testEvent", test);
//         Debug.Log("Sent");
//     }
// }
