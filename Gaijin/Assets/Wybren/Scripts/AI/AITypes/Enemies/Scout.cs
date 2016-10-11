using UnityEngine;
using System.Collections;

public class Scout
{
    //This type of enemy charges the player after [] seconds, the scout can be evaded, but does massive damage when he hits you.
    //If the scout misses the player and hits an enemy, the enemy will recieve the damage.
    //If the scout hits a wall, he will crash into it and die.

    public float speed, damage, fireRate, health, timer;

    public Transform manager, target;

    public NavMeshAgent agent;

    public bool charging;

    public Scout(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        fireRate = setFireRate;
        health = setHealth;

        agent = manager.GetComponent<NavMeshAgent>();
    }

    public void Active()
    {
        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 8 && charging == false)
        {
            agent.speed = speed;
            agent.SetDestination(target.position);
        }
        else if (distanceToTarget < 8 && distanceToTarget > 3 && charging == false && manager.GetComponent<AIManager>().visible == true)
        {
            Vector3 targetPos = target.position;
            targetPos.y = manager.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);
            manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);

            timer += 1 * Time.deltaTime;

            if (timer > fireRate)
            {
                //Charge at player
                charging = true;
                manager.LookAt(target.position);
                manager.GetComponent<Rigidbody>().velocity += manager.forward * 18;
                timer = 0;
            }
        }

        if(charging == true && manager.GetComponent<Rigidbody>().velocity.magnitude < 1)
        {
            charging = false;
        }

        if(manager.GetComponent<AIManager>().collision.transform.gameObject.tag == "Player" && charging == true)
        {
            Debug.Log("Hit");
            charging = false;
        }
        if (manager.GetComponent<AIManager>().collision.transform.gameObject.tag == "Wall")
        {
            int maxAngle = 10;
            Vector3 normal = manager.GetComponent<AIManager>().collision.contacts[0].normal;
            Vector3 vel = manager.GetComponent<Rigidbody>().velocity;
            // measure angle
            if (Vector3.Angle(vel, -normal) > maxAngle)
            {
                if (manager.GetComponent<AIManager>().collision.relativeVelocity.magnitude > 10)
                {
                    Debug.Log("Hit wall, I ded.");
                    charging = false;
                    manager.GetComponent<AIManager>().Destroy();
                }
            }
        }
    }
}