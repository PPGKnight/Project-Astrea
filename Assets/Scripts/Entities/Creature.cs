using UnityEngine;

enum CreatureType
{
    Ally,
    Enemy
}
public class Creature : MonoBehaviour
{
    #region CreatureInfo
    public string Name;
    public int Level;
    public int CurrentHP;
    public int MaxHP;
    public int CurrentMana;
    public int MaxMana;
    public sbyte SlotsTaken = 1;
    public bool HadTurn = false;
    public int SpeedBonus = 0;
    [SerializeField]
    private bool IsGuarded = false;
    [SerializeField]
    private CreatureType creatureType;
    public float tracker = 0;
    [SerializeField]
    private bool _isDead = false;
    #nullable enable
    [HideInInspector]
    public UpdateInfo? entityInfo;
    #nullable disable
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
    public string GetCreatureType()
    {
        return this.creatureType.ToString();
    }
    public int Attack()
    {
        return Random.Range(Mathf.FloorToInt((Strength + Dexterity) / 3), (Strength * 2) + Dexterity);
    }

    public void Heal(int h)
    {
        this.CurrentHP += h;
        if (this.CurrentHP > this.MaxHP) this.CurrentHP = this.MaxHP;
    }

    public void Guard()
    {
        IsGuarded = true;
    }

    public bool IsDead()
    {
        return this._isDead;
    }

    public void TakeDamage(int dmg)
    {
        this.CurrentHP -= IsGuarded ? Mathf.RoundToInt(dmg / 2) : dmg;
        IsGuarded = false;

        if(this.CurrentHP <= 0)
        {
            this._isDead = true;
        }
    }

}
