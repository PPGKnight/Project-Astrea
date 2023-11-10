using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    #region CreatureInfo
    public string Name;
    public int Level;
    public int CurrentHP;
    public int MaxHP;
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
    public Sprite avatar;

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
    public int InitiativeThisFight;
    #endregion

    private void Awake()
    {
        UpdateStats();
    }

    public virtual void UpdateStats()
    {
        MaxHP = (Constitution * 2 + Strength) * Level;
        Initiative = (Dexterity - 10) / 2;
        CurrentHP = MaxHP;
    }
    public string GetCreatureType()
    {
        return this.creatureType.ToString();
    }
    public void SetInitiative()
    {
        InitiativeThisFight = Random.Range(1, 21) + Initiative;
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


    [SerializeField] List<GameObject> outlineMaterials;
    public UnityEvent m_outlineEventOn, m_outlineEventOff;


    private void Start()
    {
        foreach (GameObject mat in outlineMaterials)
        {
            m_outlineEventOn.AddListener(mat.GetComponent<OutlineCS>().AlphaOn);
            m_outlineEventOff.AddListener(mat.GetComponent<OutlineCS>().AlphaOff);
        }
    }

    private void OnMouseOver()
    {
        m_outlineEventOn.Invoke();
    }

    private void OnMouseExit()
    {
        m_outlineEventOff.Invoke();
    }

    public string ReturnName()
    {
        return this.Name;
    }
}
