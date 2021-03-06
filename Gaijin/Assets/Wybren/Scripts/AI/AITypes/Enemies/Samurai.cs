﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Samurai
{
    public float speed, fireRate, health, timer;

    public Transform manager, target;

    NavMeshAgent agent;

    public Samurai(Transform ai, float combatSpeed, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        fireRate = setFireRate;
        health = setHealth;

        agent = manager.GetComponent<NavMeshAgent>();
    }

    public void Active()
    {
        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 3)
        {
            Move();
        }
        else
        {
            InCombat();
        }
    }

    void Move()
    {
        agent.speed = speed;
        agent.SetDestination(target.position);
    }

    void InCombat()
    {
        agent.speed = 0;
        Vector3 targetPos = target.position;
        targetPos.y = manager.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);

        // Smoothly rotate towards the target point.
        manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);

        timer += 1 * Time.deltaTime;
        if (timer > fireRate)
        {
            Attack();
            Debug.Log("attack");
            timer = 0;
        }
    }

    void Attack()
    {

    }
}
