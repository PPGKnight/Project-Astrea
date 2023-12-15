using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InitiativeTrackerManager : MonoBehaviour
{
    public Image initiativeBar;
    public Image characterImage;

    public List<Creature> characters;
    public List<Image> tokens = new List<Image>();
    List<Creature> turnCreatures;
    
    Dictionary<Creature, Image> turnCreature = new Dictionary<Creature, Image>();

    private void OnEnable()
    {
        BattleManager.ProgressTurn += UpdateTokensPosition;
        BattleManager.RemoveTokens += RemoveToken;
    }

    private void OnDisable()
    {
        BattleManager.ProgressTurn -= UpdateTokensPosition;
        BattleManager.RemoveTokens -= RemoveToken;
    }

    void SetIniBar()
    {
        int trackerSize = BattleManager.Instance.RequestTrackerOrder().Count;
        int barSize = trackerSize <= 4 ? trackerSize : 4;
        
        initiativeBar.rectTransform.sizeDelta = new Vector2(100f, barSize * 100f);
    }



    public void CreateTokens()
    {
        Debug.Log("Create");
        turnCreatures = BattleManager.Instance.RequestTrackerOrder();
        SetIniBar();
        int counter = 1;
        foreach (Creature c in turnCreatures)
        {
            int index = turnCreatures.FindIndex(x => x == c);
            Image token = Instantiate(characterImage, initiativeBar.transform);
            Vector3 r = new Vector3(0f, (-100f * index) - 100f, 0f);
            token.rectTransform.anchoredPosition = r;
            token.sprite = c.avatar;
            counter++;
            turnCreature[c] = token;
            tokens.Add(token);
        }
    }

    void UpdateTokensPosition()
    {
        StopAllCoroutines();
        List<Creature> checkOrder = BattleManager.Instance.RequestTrackerOrder();

        foreach (var c in turnCreature)
        {
            Image img = turnCreature[c.Key];
            int index = checkOrder.FindIndex(x => x == c.Key);
            RectTransform r = img.rectTransform;
            StartCoroutine(MoveToken(r, checkOrder));
        }
    }

    IEnumerator MoveToken(RectTransform r, List<Creature> checkOrder)
    {
       Vector3 newPosition;
       if (r.anchoredPosition.y >= -150f) newPosition = new Vector3(0f, (checkOrder.Count) * -100f, 0f);
       else newPosition = new Vector3(0f, r.anchoredPosition.y + 100f, 0f);

        r.DOLocalMoveY(newPosition.y, 0.5f).OnComplete(() =>
        {
            r.anchoredPosition = newPosition;
        });
        yield return null;
    }

    public void RemoveToken()
    {
        List<int> toRemove = new List<int>();
        List<KeyValuePair<Creature, Image>> toRemoveC = new List<KeyValuePair<Creature, Image>>();

        foreach (var c in turnCreature)
        {
            if (c.Key.IsDead())
            {
                int index = tokens.FindIndex(x => x == c.Value);
                toRemove.Add(index);
                toRemoveC.Add(c);
            }
        }

        foreach(int i in toRemove)
        {
            Destroy(tokens[i].gameObject);
            Vector2 newIniBar = new Vector2(initiativeBar.rectTransform.sizeDelta.x, initiativeBar.rectTransform.sizeDelta.y - 100f);
            initiativeBar.rectTransform.sizeDelta = newIniBar;
        }

        foreach(var c in toRemoveC)
        {
            tokens.Remove(c.Value);
            turnCreature.Remove(c.Key);
        }

        toRemove.Clear();
        toRemoveC.Clear();

        List<Creature> tempList = BattleManager.Instance.RequestTrackerOrder();

        foreach (var c in turnCreature)
        {
            int temp = tempList.FindIndex(x => x == c.Key);
            Image i = turnCreature[c.Key];
            Vector3 newPos = new Vector3(0f, (-100f * temp) - 100f, 0f);
            i.rectTransform.anchoredPosition = newPos;
        }
    }
}