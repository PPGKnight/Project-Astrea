using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateInfo : MonoBehaviour
{
    [SerializeField] Image _characterImage;
    [SerializeField] TMP_Text _characterName;
    [SerializeField] TMP_Text _characterLevel;
    [SerializeField] Slider _characterSliderHP;
    [SerializeField] TMP_Text _characterTextHP;
    [SerializeField] Slider _characterSliderMana;
    [SerializeField] TMP_Text _characterTextMana;

    public void SetInfo(string charName, int charLv, int charCHP, int charMHP, int charCM, int charMM)
    {
        //Debug.Log("Zosta³em wykonany A");
        this._characterName.text = charName;
        this._characterLevel.text = $"Lv. {charLv}";
        this._characterSliderHP.maxValue = charMHP;
        this._characterSliderHP.value = charCHP;
        this._characterTextHP.text = $"{charCHP} / {charMHP}";
        this._characterSliderMana.maxValue = charMM;
        this._characterSliderMana.value = charCM;
        this._characterTextMana.text = $"{charCM} / {charMM}";
    }

    public void SetEnemyInfo(string charName, int charCHP, int charMHP)
    {
        //Debug.Log("Zosta³em wykonany");
        this._characterName.text = charName;
        this._characterSliderHP.maxValue = charMHP;
        this._characterSliderHP.value = charCHP;
    }

    public void UpdateHP(int c)
    {
        //Debug.Log($"Set to {c}");
        this._characterSliderHP.value = c;
    }
}
