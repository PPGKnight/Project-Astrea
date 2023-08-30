using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager instance;
    public static DialogueManager Instance {  
        get { return instance; } 
    }

    [Header("UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject continueIcon;

    Story currentStory;
    public bool isDialogue { get; private set; }
    bool canContinueStory = false;
    Coroutine goToNextLine;

    [Header("Choices")]
    [SerializeField] GameObject[] choices;
    TextMeshProUGUI[] choicesText;

    PlayerInput input;
    InputAction submitAction;
    Vector2 moveAction;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Additional Dialogue Manager has been deleted from the scene");
            Destroy(this.gameObject);
        }
            instance = this;

        input = GetComponent<PlayerInput>();
        submitAction = input.actions["Submit"];
    }

    private void Start()
    {
        isDialogue = false;
        dialoguePanel.SetActive(false);
        GameManager.Instance.worldTime = 0;

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

        moveAction = input.actions["Move"].ReadValue<Vector2>();
        submitAction.performed += _ =>
        {
            if (currentStory.currentChoices.Count == 0 && canContinueStory) ContinueStory();
        };
    }

    public void EnterDialogue(TextAsset ink)
    {
        currentStory = new Story(ink.text);
        isDialogue = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    void ContinueStory()
    {
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
                goToNextLine = StartCoroutine(DisplayLine(newLine));
        }
        else
            StartCoroutine(ExitDialogue());
    }

    IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.worldTime = 1;

        isDialogue = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
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
}
