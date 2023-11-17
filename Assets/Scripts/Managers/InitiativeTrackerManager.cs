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
    private List<Creature> aliveCharacters;
    public List<Image> tokens = new List<Image>();
    bool UpdateTokens = false;

    private float initiativeBarWidth;
    private float initiativeBarHeight;

    void SetIniBar()
    {
        int barSize = aliveCharacters.Count <= 5 ? aliveCharacters.Count : 5;
        //int trackerSize = BattleManager.Instance.RequestTrackerOrder().Count;
        //int barSize = trackerSize <= 5 ? trackerSize : 5;
        initiativeBar.rectTransform.sizeDelta = new Vector2(100f, barSize * 100f);
        //initiativeBarWidth = initiativeBar.rectTransform.sizeDelta.x;
        initiativeBarHeight = initiativeBar.rectTransform.sizeDelta.y;
    }

    private void OnEnable()
    {
        BattleManager.ProgressTurn += UpdateTokensPosition;
    }

    private void OnDisable()
    {
        BattleManager.ProgressTurn -= UpdateTokensPosition;
    }

    public void CreateTokens(List<Creature> _creatures)
    {
        characters = new List<Creature>(_creatures);
        aliveCharacters = new List<Creature>(characters);
        SetIniBar();
        int counter = 1;
        foreach (Creature c in aliveCharacters)
        {
            Image token = Instantiate(characterImage,initiativeBar.transform);
            Vector3 r = new Vector3(0, -100f * counter, 0f);
            token.rectTransform.anchoredPosition = r;
            token.sprite = c.avatar;
            counter++;
            tokens.Add(token);
        }

        //UpdateTokensPosition();
    }


    public void RemoveToken()
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(characters[i].IsDead())
            {
               aliveCharacters.RemoveAt(i);
               Destroy(tokens[i].gameObject);
               tokens.RemoveAt(i);
               characters.RemoveAt(i);
            }
        }

        SetIniBar();
        UpdateTokensPosition();
    }
    /*
     * Prototype function
    void UpdateTokensPosition()
    {
        for (int i = 0; i < aliveCharacters.Count; i++)
        {
            Creature c = aliveCharacters[i];
            float tokenPosition = (c.tracker / 100f) * (initiativeBarWidth - 50f);
            tokens[i].rectTransform.anchoredPosition = new Vector2(tokenPosition, 0f);
        }
    }
    */
    void UpdateTokensPosition()
    {
        for (int i = 0; i < aliveCharacters.Count; i++)
        {
            RectTransform r = tokens[i].rectTransform;
            Vector3 newPosition = new Vector3(0f, r.anchoredPosition.y + 100f, 0f);
            r.anchoredPosition = newPosition;

            if (r.anchoredPosition.y >= 0f)
                r.anchoredPosition = new Vector3(0f, aliveCharacters.Count * -100f, 0f);
        }
    }

    /*
     * TO DO
    List<Creature> turnCreatures;

    public void CreateTokens()
    {
        turnCreatures = BattleManager.Instance.RequestTrackerOrder();
        SetIniBar();
        int counter = 1;
        foreach (Creature c in turnCreatures)
        {
            Image token = Instantiate(characterImage, initiativeBar.transform);
            Vector3 r = new Vector3(0f, 100f * counter, 0f);
            token.rectTransform.anchoredPosition = r;
            token.sprite = c.avatar;
            counter++;
            tokens.Add(token);
        }
    }

    void UpdateTokensPosition()
    {
        int counter = 1;
        foreach (Image img in tokens)
        {
            RectTransform r = img.rectTransform;
            int nextIndexPosition = img.GetComponent<Creature>.
            Vector3 newPosition = new Vector3(0f, r.anchoredPosition.x - 100f, 0f);
        }
    }
    */
}