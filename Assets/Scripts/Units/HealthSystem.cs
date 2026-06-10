using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnUnitDeath;
    public event EventHandler OnHealthChanged;
        
    private int currentHealth;
    private int maximumHealth = 100;

    private void Awake()
    {
        currentHealth = maximumHealth;
    }

    public void TakeDamage(int damageIncome)
    {
        currentHealth -= damageIncome;
        
        if (currentHealth <= 0) currentHealth = 0;
        if (currentHealth == 0)
        {
            UnitDeath();
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UnitDeath()
    {
        OnUnitDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetCurrentHealthInFloat()
    {
        return (float)currentHealth / maximumHealth;
    }
}

