using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeTrackerManager : MonoBehaviour
{
    public Image initiativeBar;
    public Image characterImage;
    public List<Creature> characters;
    private List<Creature> aliveCharacters;
    public List<Image> tokens = new List<Image>();

    private float initiativeBarWidth;

    void Start()
    {
        initiativeBarWidth = initiativeBar.rectTransform.sizeDelta.x;

        UpdateTokensPosition();
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
        foreach (Creature c in aliveCharacters)
        {
            Image token = Instantiate(characterImage, initiativeBar.transform);
            token.sprite = c.avatar;
            tokens.Add(token);
        }
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

    }

    void UpdateTokensPosition()
    {
        for (int i = 0; i < aliveCharacters.Count; i++)
        {
            Creature c = aliveCharacters[i];
            float tokenPosition = (c.tracker / 100f) * (initiativeBarWidth - 50f);
            tokens[i].rectTransform.anchoredPosition = new Vector2(tokenPosition, 0f);
        }
    }
}
