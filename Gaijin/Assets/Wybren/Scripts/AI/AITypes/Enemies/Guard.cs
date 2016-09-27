using UnityEngine;
using System.Collections;

public class Guard
{
    public float speed, damage, health;

    public Transform manager;

    public Guard(Transform ai, float combatSpeed, float setDamage, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        health = setHealth;
    }

    public void Active ()
    {

    }
}
