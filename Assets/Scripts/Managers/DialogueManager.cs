using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager instance;
    public static DialogueManager Instance {  
        get { return instance; } 
    }

    public DialogueList dialogueList;

    public static event Action CheckDialogues;

    public void CheckRequirements()
    {
        CheckDialogues?.Invoke();
    }

    [Header("UI")]
    [SerializeField] GameObject dialogueEventSystem;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] GameObject continueIcon;

    Story currentStory;
    public bool isDialogue { get; private set; } = false;
    bool canContinueStory = false;
    Coroutine goToNextLine;
    Player player;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string TELEPORT_TAG = "teleport";
    private const string FIGHT_TAG = "fight";
    private const string OUTCOME_TAG = "changeOutcome";

    [Header("Choices")]
    [SerializeField] GameObject[] choices;
    TextMeshProUGUI[] choicesText;

    PlayerInput input;
    GameManager gameManager;
    InputAction submitAction;
    Vector2 moveAction;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Additional Dialogue Manager has been deleted from the scene");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if(player == null)
            player = GameObject.FindGameObjectWithTag("MainPlayer").GetComponentInChildren<Player>();

        if (gameManager == null)
            gameManager = GameManager.Instance;

        dialogueList = new DialogueList();
        input = GetComponent<PlayerInput>();
        submitAction = input.actions["Submit"];
    }

    private void Start()
    {
        isDialogue = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void Update()
    {
        if (!isDialogue) return;

        if(player == null)
            player = GameObject.FindGameObjectWithTag("MainPlayer").GetComponentInChildren<Player>();

        if (gameManager == null)
            gameManager = GameManager.Instance;

        moveAction = input.actions["Move"].ReadValue<Vector2>();
        submitAction.performed += _ =>
        {
            if( isDialogue )
                if (currentStory.currentChoices.Count == 0 && canContinueStory) 
                    ContinueStory();
        };
    }

    string dialogue_name;
    public void EnterDialogue(TextAsset ink)
    {
        GameManager.Instance.worldTime = 0;
        Debug.Log("Rozpoczynam dialog");
        currentStory = new Story(ink.text);
        Debug.Log("Story zaczytane");
        if (player == null) Debug.Log("Nie ma playera");
        object playername = player.ReturnName();
        
        if (currentStory.variablesState["PlayerName"] != null)
            currentStory.variablesState["PlayerName"] = playername;

        dialogue_name = currentStory.variablesState["DialogueID"].ToString();
        Debug.Log("Nazwa dialogu wzieta");

        if (currentStory.variablesState["outcome"] != null)
            currentStory.variablesState["outcome"] = outcome;

        isDialogue = true;
        dialoguePanel.SetActive(true);
        dialogueEventSystem.SetActive(true);
        Debug.Log("Canvas wlaczone");

        ContinueStory();
    }

    void ContinueStory()
    {
        //Debug.Log("Kontynuuje");
        if (currentStory.canContinue)
        {
            if (goToNextLine != null)
            {
                StopCoroutine(goToNextLine);
            }

            string newLine = currentStory.Continue();

            if (newLine.Equals("") && !currentStory.canContinue)
                StartCoroutine(ExitDialogue());
            else
            {
                //Debug.LogError(currentStory.currentTags[0]);
                HandleTags(currentStory.currentTags);
                goToNextLine = StartCoroutine(DisplayLine(newLine));
            }
        }
        else
            StartCoroutine(ExitDialogue());
    }

    IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);

        if(dialogue_name != null && dialogue_name != "")
            dialogueList.UpdateDialogue(dialogue_name);
        GameManager.Instance.worldTime = 1;

        isDialogue = false;
        dialoguePanel.SetActive(false);
        //dialogueEventSystem.SetActive(false);
        dialogueText.text = "";
        currentStory = null;
        canFight = true;
        dialogue_name = "";
        dialogueSpeaker.text = "";
    }

    IEnumerator DisplayLine(string nextLine)
    {
        yield return new WaitForSeconds(0.1f);
        dialogueText.text = nextLine;
        dialogueText.maxVisibleCharacters = 0;
        HideChoices();
        canContinueStory = false;

        bool isSkipped = false;
        foreach ( char letter in nextLine.ToCharArray())
        {
            submitAction.performed += _ =>
            {
                dialogueText.maxVisibleCharacters = nextLine.Length;
                isSkipped = true;
            };
            if (isSkipped) break;

            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.07f);
        }

        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueStory = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    dialogueSpeaker.text = tagValue.Replace("_", " ");
                    break;
                case OUTCOME_TAG:
                    outcome = Int32.Parse(tagValue);
                    break;
                case FIGHT_TAG:
                    GameObject o = GameObject.Find(tagValue);
                    Debug.Log($"Uruchamiam walke {o.name}");
                    StartCoroutine(PrepareToFight(o));
                    break;
                case TELEPORT_TAG:
                    string[] strings = tagValue.Split('_');
                    Debug.LogWarning($"{strings[0]} {strings[1]}");
                    PlayerMovement getobject = GameObject.FindGameObjectWithTag(strings[0]).GetComponent<PlayerMovement>();
                    if (getobject == null)
                    {
                        getobject = GameObject.FindGameObjectWithTag(strings[0]).GetComponent<PlayerMovement>();
                        Debug.LogError("PlayerMovement null");

                        if (GameManager.Instance._player == null) Debug.LogError("GM Player null");
                        else getobject = GameManager.Instance._player.GetComponent<PlayerMovement>();
                    }
                    TeleportTo(getobject, TransitionSpawns.ReturnSpawn(strings[1]));
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    bool canFight = false;
    IEnumerator PrepareToFight(GameObject o)
    {
        while (!canFight)
        {
            yield return null;
        }
        o.GetComponent<EnemyEncounter>().UpdateReady();
        StopFightCoroutine(o);
    }

    void StopFightCoroutine(GameObject o)
    {
        StopCoroutine(PrepareToFight(o));
        canFight = false;
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int index)
    {
        if (canContinueStory)
        {
            currentStory.ChooseChoiceIndex(index);
            ContinueStory();
        }
    }

    void HideChoices()
    {
        foreach (GameObject go in choices)
            go.SetActive(false);
    }

    void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("There is more choices than UI can handle");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    void TeleportTo(PlayerMovement obj, Vector3 loc)
    {
        Debug.LogWarning($"Teleportuje {obj.name} na koordynaty {loc}");
        //obj.transform.position = loc;
        obj.UpdateAgent(loc);
    }
    
    int outcome = 0;
}
