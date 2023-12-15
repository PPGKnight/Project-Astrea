using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthBarSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    private int healthAmount;
    private int healthAmountMax;
    private Player _player;
    private int previousHealth;
    public HealthBarSystem(Player p)
    {
        this._player = p;
        this.healthAmount = p.CurrentHP;
        this.healthAmountMax = p.MaxHP;
        this.previousHealth = p.CurrentHP;
        BattleManager.UpdateBars += UpdateHP;
    }
    void UpdateHP()
    {
        if (this._player.CurrentHP != this.previousHealth)
        {
            if (this._player.CurrentHP < this.previousHealth)
                Damage(this.previousHealth - this._player.CurrentHP);
            else
                Heal(this._player.CurrentHP - this.previousHealth);

            this.previousHealth = this._player.CurrentHP;
        }
    }

    public void Damage(int amount)
    {
        healthAmount -= amount;
        if (healthAmount < 0)
            healthAmount = 0;
        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }
    public void Heal(int amount)
    {
        healthAmount += amount;
        if (healthAmount > healthAmountMax)
        {
            healthAmount = healthAmountMax;
        }
        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }
    public float GetHealthNormalized()
    {
        float f = 0f;
        f = (float)healthAmount / (float)healthAmountMax;
        return f;
    }
}