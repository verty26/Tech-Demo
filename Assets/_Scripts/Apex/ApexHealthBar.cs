using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApexHealthBar : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int shield;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;

    private const int MaxHealth = 100;
    private const int MaxShield = 5; // Max shield value

    private void Start()
    {
        // Initialize sliders with clamped values
        health = Mathf.Clamp(health, 0, MaxHealth);
        shield = Mathf.Clamp(shield, 0, MaxShield);

        healthSlider.maxValue = MaxHealth;
        shieldSlider.maxValue = MaxShield;

        healthSlider.value = health;
        shieldSlider.value = shield;
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, MaxHealth);
        healthSlider.value = health;
    }

    public void AddShield(int amount)
    {
        shield = Mathf.Clamp(shield + amount, 0, MaxShield);
        shieldSlider.value = shield;
    }

    public void TakeDamage(int amount)
    {
        // Calculate how many shield points to reduce based on the damage amount
        int shieldDamage = Mathf.FloorToInt(amount / 25f); // Every 25 damage equals 1 shield
        int remainingDamage = amount % 25; // Leftover damage after reducing shield

        if (shield > 0)
        {
            if (shieldDamage >= shield)
            {
                // All shield points are consumed
                remainingDamage += (shieldDamage - shield) * 25; // Convert leftover shield damage back to raw damage
                shield = 0;
            }
            else
            {
                // Partially reduce the shield
                shield -= shieldDamage;
            }
        }
        else
        {
            // Apply remaining damage to health
            health = Mathf.Clamp(health - amount, 0, MaxHealth);
        }

        

        // Update sliders
        healthSlider.value = health;
        shieldSlider.value = shield;
    }
}
