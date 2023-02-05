using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string charName;

    public int damagePower;
    public int healPower;
    public int specialDamagePower;
    public int specialMaxCooldown;
    public int specialCurrentCooldown;
    public int restPower;


    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void Rest(int amount)
    {
        specialCurrentCooldown -= amount;
        if (specialCurrentCooldown < 0)
        {
            specialCurrentCooldown = 0;
        }
    }
}
