using UnityEngine;

public class ShowMarker : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] string questName;

    private void OnEnable() => GameEventsManager.instance.QuestEvents.onQuestStateChange += CheckMarker;
    private void OnDisable() => GameEventsManager.instance.QuestEvents.onQuestStateChange -= CheckMarker;
    void Start() => sr = GetComponent<SpriteRenderer>();

    void CheckMarker(Quest q)
    {
        if (q.info.id ==  questName && q.state == QuestState.In_Progress)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        else
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    }
}
