﻿using UnityEngine;
using System.Collections;

public class Assasin
{
    public float speed, damage, health;

    public Transform manager, target;

    public Assasin(Transform ai, float combatSpeed, float setDamage, float setHealth)
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