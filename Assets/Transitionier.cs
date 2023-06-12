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
        {
            GameManager.Instance.activeScenes.Clear();
            GameManager.Instance.activeScenes.Add(_nextSceneName);
            SceneManager.LoadScene(_nextSceneName);
            //GameManager.Instance.SpawnThePlayer(false);
            GameManager.Instance._player.GetComponent<PlayerMovement>().UpdateAgent(safeSpawns[key]);
        }

        else if (_trans == TransitionType.Additive && !GameManager.Instance.activeScenes.Contains(_nextSceneName))
        {
            GameManager.Instance.activeScenes.Add(_nextSceneName);
            //SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextSceneName));
            StartCoroutine(Load());
        }
        Debug.Log(safeSpawns[key]);
        GameManager.Instance.MovePlayerToNewPos();
    }
    IEnumerator MovePlayer()
    {
        yield return null;
    }
    IEnumerator Load()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
        while(!asyncLoad.isDone)
        { yield return null; }    
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextSceneName));
        yield return null;
    }

    void UnloadScene()
    {
        if (GameManager.Instance.activeScenes.Contains(_previousSceneName))
        {
            GameManager.Instance._playerPositionSO.SetPosition(GameManager.Instance._player.transform.position, GameManager.Instance._player.transform.rotation, GameManager.Instance._player.transform.localScale);

            GameManager.Instance.activeScenes.Remove(_previousSceneName);
            SceneManager.UnloadSceneAsync(_previousSceneName);
        }
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
        {"InnToHendleyVillage", new Vector3(-490.5736f, -1.550712f, -693.4078f)},
        {"HendleyVillageToInn", new Vector3(-9.897679f, 1f, 6.759513f)},
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