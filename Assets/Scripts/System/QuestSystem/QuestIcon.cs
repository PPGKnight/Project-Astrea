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
        cantStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        cantFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        switch (newState)
        {
            case QuestState.Requirements_Not_Met:
                if (startPoint) { cantStartIcon.SetActive(true); }
                break;
            case QuestState.Can_Start:
                if (startPoint) { canStartIcon.SetActive(true); }
                break;
            case QuestState.In_Progress:
                if (finishPoint) { cantFinishIcon.SetActive(true); }
                break;
            case QuestState.Can_Finish:
                if (finishPoint) { canFinishIcon.SetActive(true); }
                break;
            case QuestState.Completed:
                break;
            case QuestState.Hidden:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}