using UnityEngine;
using System.Collections;

public class Archer
{
    public float speed, damage, health;

    public Transform manager;

    public Archer(Transform ai, float combatSpeed, float setDamage, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        health = setHealth;
    }

    public void Active()
    {

    }
}
