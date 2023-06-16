using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarShrink : MonoBehaviour
{
    public AllyPosition _allyPosition;
    private const float DAMAGED_HEALTH_TIMER_MAX = 1f;
    private Image barImage;
    private Image damagedBarImage;
    private float damagedHealthShrinkTimer;
    private HealthBarSystem healthSystem;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
    }

    private void Start()
    {
        Player? ally = GameObject.Find("Ally"+(int)_allyPosition).GetComponentInChildren<Player>();
        Debug.Log($"Kurwo {ally}");
        if (ally != null)
        {
            gameObject.SetActive(true);
            Debug.Log(ally);
            healthSystem = new HealthBarSystem(ally);
            SetHealth(healthSystem.GetHealthNormalized());
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnHealed += HealthSystem_OnHealed;
            damagedBarImage.fillAmount = barImage.fillAmount;
        }
        else
            gameObject.SetActive(false);
    }

    private void Update()
    {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if(damagedHealthShrinkTimer <0)
        {
            if(barImage.fillAmount < damagedBarImage.fillAmount)
            {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        damagedHealthShrinkTimer = DAMAGED_HEALTH_TIMER_MAX;
        Debug.Log($"Shrinking to {healthSystem.GetHealthNormalized()}");
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float healthNormalized)
    {
        barImage.fillAmount = healthNormalized;
    }
}

public enum AllyPosition
{
    Position_1 = 1,
    Position_2 = 2,
    Position_3 = 3,
    Position_4 = 4
}