using UnityEngine;
using System.Collections;

public class Guard
{
    public float speed, damage, fireRate, health;

    public Transform manager, target;

    NavMeshAgent agent;

    public Guard(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        fireRate = setFireRate;
        health = setHealth;

        agent = manager.GetComponent<NavMeshAgent>();
    }

    public void Active ()
    {
        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 3)
        {
            agent.speed = speed;
            agent.SetDestination(target.position);
        } 
        else
        {
            agent.speed = 0;
            Vector3 targetPos = target.position;
            targetPos.y = manager.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);

            // Smoothly rotate towards the target point.
            manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);
        }    
    }
}
