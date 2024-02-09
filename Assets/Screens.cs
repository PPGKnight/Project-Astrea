using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Screens : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuScreen, thankYouScreen, gameOverScreen;
    bool isAnyScreenActive = false;
    GameObject activeScreen;

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.onFinishQuest += IsEndOfDemo;
        GameEventsManager.instance.MiscEvents.onDeathScreen += Death;
        GameEventsManager.instance.MiscEvents.onOptionKeyPressed += OptionKeyPressed;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.onFinishQuest -= IsEndOfDemo;
        GameEventsManager.instance.MiscEvents.onDeathScreen -= Death;
        GameEventsManager.instance.MiscEvents.onOptionKeyPressed -= OptionKeyPressed;
    }

    PlayerInput input;
    InputAction debugppm;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        debugppm = input.actions["DebugPPM"];
    }

    private void Update()
    {
        debugppm.performed += _ =>
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, results);
            foreach (var r in results)
                Debug.Log(r.gameObject.name);
        };
    }

    void ShowScreen(GameObject go)
    {
        if (DialogueManager.Instance.isDialogue) return;
        if (activeScreen == gameOverScreen) return;
        if (isAnyScreenActive) HideScreen(activeScreen);

        GameManager.Instance.worldTime = 0;
        GameManager.Instance.isPaused = true;
        activeScreen = go;
        isAnyScreenActive = true;
        go.SetActive(true);
        StopAllCoroutines();
    }
    public void HideScreen(GameObject go)
    {
        go.SetActive(false);

        if(!DialogueManager.Instance.isDialogue)
            GameManager.Instance.worldTime = 1;
        
        GameManager.Instance.isPaused = false;
        activeScreen = null;
        isAnyScreenActive = false;
    }

    void IsEndOfDemo(string quest)
    {
        if (quest == "002_CheckLumbererForTheSmith")
            if (DialogueManager.Instance.isDialogue)
                StartCoroutine(WaitForDialogueToEnd());
            else
                ShowScreen(thankYouScreen);
    }

    IEnumerator WaitForDialogueToEnd()
    {
        while (DialogueManager.Instance.isDialogue)
        {
            yield return null; 
        }
        ShowScreen(thankYouScreen);
    }

    public void OptionKeyPressed()
    {
        if (activeScreen == pauseMenuScreen) HideScreen(pauseMenuScreen);
        else ShowScreen(pauseMenuScreen);
    }

    public void Death() => ShowScreen(gameOverScreen);

    public void LeaveToMenu()
    {
        GameObject m = GameObject.FindGameObjectWithTag("Managers");
        GameObject p = GameObject.FindGameObjectWithTag("PlayerHolder");
        SceneManager.LoadScene(0);
        Destroy(p);
        Destroy(m);
    }
}