using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health  {

    float maxHealth;
    float currentHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public Health(float value)
    {
        maxHealth = value;
        SetHealthToMax();
    }
  

    public bool isAlive()
    {
        if (currentHealth > 0)
            return  true;
        else
            return false;
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
    }

    public void Heal (float value)
    {
        currentHealth += value;
    }

    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
    }


}
