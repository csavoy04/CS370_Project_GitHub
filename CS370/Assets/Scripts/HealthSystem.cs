using System;
using UnityEngine;
public class HealthSystem
{
    
    public event EventHandler OnHealthChanged;
    public int MaxHealth;
    public int CurrentHealth;

    public HealthSystem(int MaxHealth)
    {
        this.MaxHealth = MaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void GetHealth()
    {
        Debug.Log("Current Health: " + CurrentHealth + "/" + MaxHealth);
    }
    public float GetHealthPercent()
    {
        return (float)CurrentHealth / MaxHealth;
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
