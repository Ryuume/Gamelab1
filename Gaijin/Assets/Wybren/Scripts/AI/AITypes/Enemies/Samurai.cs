using UnityEngine;
using System.Collections;

public class Samurai
{
    public float speed, damage, fireRate, health;

    public Transform manager, target;

    public Samurai(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        fireRate = setFireRate;
        health = setHealth;
    }

    public void Active()
    {

    }
}
