using UnityEngine;
using System.Collections;

public class Guard
{
    public float speed, damage, health;

    public Transform manager, target;

    NavMeshAgent agent;

    public Guard(Transform ai, float combatSpeed, float setDamage, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        health = setHealth;

        agent = manager.GetComponent<NavMeshAgent>();
    }

    public void Active ()
    {
        

        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 2)
        {
            agent.speed = speed;
            agent.SetDestination(target.position);
        } 
        else
        {
            agent.speed = 0;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - manager.position);

            // Smoothly rotate towards the target point.
            manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 10 * Time.deltaTime);
        }    
    }
}
