using UnityEngine;

public class Creature : MonoBehaviour
{
    #region CreatureInfo
    public string Name;
    public int Level;
    public int CurrentHP;
    public int MaxHP;
    public int CurrentMana;
    public int MaxMana;
    public bool IsDead = false;
    public sbyte SlotsTaken = 1;
    public bool HadTurn = false;
    public int SpeedBonus = 0;
    #endregion

    #region Attributes
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Constitution;
    public int Initiative;
    #endregion

    private void Awake()
    {
        UpdateStats();
    }

    public virtual void UpdateStats()
    {
        MaxHP = (Constitution * 2 + Strength) * Level;
        MaxMana = Intelligence * 3 * Level;
        Initiative = Mathf.RoundToInt(((Dexterity - 10)/2)+SpeedBonus);
        CurrentHP = MaxHP;
        CurrentMana = MaxMana;
    }
}
