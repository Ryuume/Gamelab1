using UnityEngine;
using System.Collections;

public class Assasin
{
    public float speed, damage, fireRate, health;

    public Transform manager, target;

    public Assasin(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
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
