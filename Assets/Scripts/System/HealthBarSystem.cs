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

    public HealthBarSystem(int healthAmount)
    {
        this.healthAmount = healthAmount;
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
        return (float)healthAmount / healthAmountMax;
    }
}
