using UnityEngine;
using System.Collections;

public class Scout
{
    public float speed, damage, health;

    public Transform manager;

    public Scout(Transform ai, float combatSpeed, float setDamage, float setHealth)
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
