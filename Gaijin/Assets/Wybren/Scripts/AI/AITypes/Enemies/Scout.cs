using UnityEngine;
using System.Collections;

public class Scout
{
    //This type of enemy charges the player after [] seconds, the scout can be evaded, but does massive damage when he hits you.
    //If the scout misses the player and hits an enemy, the enemy will recieve the damage.
    //If the scout hits a wall, he will crash into it and die.

    public float speed, fireRate, health, timer, timer2;

    public Transform manager, target;

    public NavMeshAgent agent;

    public bool charging;

    public Scout(Transform ai, float combatSpeed, float setFireRate, float setHealth)
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

        if (distanceToTarget > 6 && charging == false)
        {
            agent.speed = 5;
            agent.SetDestination(target.position);
        }
        else if (distanceToTarget < 7 && distanceToTarget > .5f && charging == false && manager.GetComponent<AIManager>().visible == true)
        {
            Vector3 targetPos = target.position;
            targetPos.y = manager.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);
            manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);

            timer += 1 * Time.deltaTime;

            if (timer > fireRate && manager.GetComponent<AIManager>().dead != true)
            {
                //Charge at player
                
                manager.LookAt(target.position);

                Animator animator = manager.GetComponent<AIManager>().animator;
                manager.GetComponent<AIManager>().weapon.SendMessageUpwards("DoDamage");
                animator.SetTrigger("Attack");
                Debug.Log("Charge");
                if(distanceToTarget > .6)
                {
                    charging = true;
                    manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    manager.GetComponent<Rigidbody>().velocity += manager.forward * 18;
                }
                
                timer = 0;
            }
        }

        if (charging == true && manager.GetComponent<Rigidbody>().velocity.magnitude > 1)
        {
            timer2 += Time.deltaTime;
            if(timer2 > 1f)
            {
                charging = false;
                timer2 = 0;
                manager.GetComponent<Rigidbody>().velocity = Vector3.zero;
                manager.GetComponent<AIManager>().weapon.SendMessageUpwards("NoDamage");
                manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                manager.GetComponent<Rigidbody>().freezeRotation = true;
            }
        }

        if (charging == true && manager.GetComponent<Rigidbody>().velocity.magnitude < 1)
        {
            manager.GetComponent<Rigidbody>().velocity = Vector3.zero;
            manager.GetComponent<AIManager>().weapon.SendMessageUpwards("NoDamage");
            manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            manager.GetComponent<Rigidbody>().freezeRotation = true;
            charging = false;
        }

        if(manager.GetComponent<AIManager>().collision.transform.gameObject.tag == "Player" && charging == true)
        {
            Debug.Log("Hit");
            manager.GetComponent<Rigidbody>().velocity = Vector3.zero;
            manager.GetComponent<AIManager>().weapon.SendMessageUpwards("NoDamage");
            manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            manager.GetComponent<Rigidbody>().freezeRotation = true;
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
                    //Debug.Log("Hit wall, I ded.");
                    manager.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    manager.GetComponent<AIManager>().weapon.SendMessageUpwards("NoDamage");
                    manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    manager.GetComponent<Rigidbody>().freezeRotation = true;
                    charging = false;
                    //manager.GetComponent<AIManager>().Destroy();
                }
            }
        }
    }
}