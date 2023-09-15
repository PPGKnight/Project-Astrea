using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject cantStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject cantFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        cantStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        cantFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case QuestState.Requirements_Not_Met:
                if (startPoint) { cantStartIcon.SetActive(true); }
                Debug.Log("Not Met");
                break;
            case QuestState.Can_Start:
                if (startPoint) { canStartIcon.SetActive(true); }
                Debug.Log("C Start");
                break;
            case QuestState.In_Progress:
                if (finishPoint) { cantFinishIcon.SetActive(true); }
                Debug.Log("I Prog");
                break;
            case QuestState.Can_Finish:
                if (finishPoint) { canFinishIcon.SetActive(true); }
                Debug.Log("C Fin");
                break;
            case QuestState.Completed:
                Debug.Log("Complete");
                break;
            case QuestState.Hidden:
                Debug.Log("Hidden");
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}