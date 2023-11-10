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

    void SetIniBar()
    {
        int size = aliveCharacters.Count <= 5 ? aliveCharacters.Count : 5;
        initiativeBar.rectTransform.sizeDelta = new Vector2(size * 100f, 100f);
        initiativeBarWidth = initiativeBar.rectTransform.sizeDelta.x;
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
            Vector3 r = new Vector3(100f * counter, 0f, 0f);
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
            Vector3 newPosition = new Vector3(r.anchoredPosition.x - 100f, 0f, 0f);
            r.anchoredPosition = newPosition;

            if (r.anchoredPosition.x < 0f)
                r.anchoredPosition = new Vector3((aliveCharacters.Count - 1) * 100f, 0f);
        }
    }

}