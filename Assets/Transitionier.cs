using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitionier : MonoBehaviour
{
    [SerializeField]
    TransitionType _trans;

    [SerializeField]
    ActionType _action;

    [SerializeField]
    string _previousSceneName;

    [SerializeField]
    string _nextSceneName;

    string key;

    void LoadScene()
    {
        GameManager.Instance.ChangePlayerPos(safeSpawns[key]);

        if (_trans == TransitionType.Normal)
            SceneManager.LoadScene(_nextSceneName);

        else if (_trans == TransitionType.Additive)
            SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Additive);
    }

    void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(_previousSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        key = "";
        key = _previousSceneName + "To" + _nextSceneName;


        if (other.CompareTag("MainPlayer"))
            if (_action == ActionType.Load)
                LoadScene();
            else
                UnloadScene();
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (_trans != TransitionType.Additive) return;

        if (other.CompareTag("MainPlayer"))
            UnloadScene();
    }
    */

    Dictionary<string, Vector3> safeSpawns = new Dictionary<string, Vector3>()
    {
        {"InnToInn_SleepingRooms", new Vector3(0f, 1f, -1.5f)},
        {"Inn_SleepingRoomsToInn", new Vector3(5.3f, 3f, 1f)},
        {"InnToHendleyVillage", new Vector3(54f, 2f, 15f)},
        {"HendleyVillageToInn", new Vector3(-10f, 2f, 7f)},
        {"HendleyVillageToRoadToHendley", new Vector3(-230f, -3f, -275f)},
        {"RoadToHendleyToHendleyVillage", new Vector3(76f, 2f, 76f)},
    };
}

enum TransitionType
{
    Normal,
    Additive
}

enum ActionType
{
    Load,
    Unload
}