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
    WithLoadingScreen _wLS;

    [SerializeField]
    string _previousSceneName;

    [SerializeField]
    string _nextSceneName;

    string key;

    void LoadScene()
    {
        //QuestManager.Instance.SaveAllQuests();
        GameManager.Instance.ChangePlayerPos(TransitionSpawns.ReturnSpawn(key));
        if (_trans == TransitionType.Normal)
        {
            GameManager.Instance.activeScenes.Clear();
            GameManager.Instance.activeScenes.Add(_nextSceneName);
            SceneManager.LoadScene(_nextSceneName);
            //GameManager.Instance.SpawnThePlayer(false);
            GameManager.Instance._player.GetComponent<PlayerMovement>().UpdateAgent(TransitionSpawns.ReturnSpawn(key));
        }

        else if (_trans == TransitionType.Additive && !GameManager.Instance.activeScenes.Contains(_nextSceneName))
        {
            GameManager.Instance.activeScenes.Add(_nextSceneName);
            //SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nextSceneName));
            StartCoroutine(Load());
        }
        Debug.Log(TransitionSpawns.ReturnSpawn(key));
        GameManager.Instance.MovePlayerToNewPos();
        //QuestManager.Instance.LoadAllQuests();
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
                if(_wLS == WithLoadingScreen.YES)
                    LoadingScreenManager.Instance.LoadSceneWithMove(_nextSceneName, key);
                else
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
}

public enum WithLoadingScreen
{
    NO,
    YES
}