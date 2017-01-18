using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Assasin
{
    public float speed, fireRate, health, timer, shortestDistance, projectileSpeed = 20f;

    public List<float> distances = new List<float>();
    public List<Vector3> locations = new List<Vector3>();

    int locationIndex;

    public Transform manager, target;

    public List<Vector3> directions = new List<Vector3>();

    public bool recalculatedPath;

    Vector3 latestPos, walkTarget;

    NavMeshAgent agent;

    LayerMask obstacleMask;

    public Assasin(Transform ai, float combatSpeed, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        fireRate = setFireRate;
        health = setHealth;

        directions.Add(new Vector3(1, 0, 0));
        directions.Add(new Vector3(-1, 0, 0));
        directions.Add(new Vector3(0, 0, 1));
        directions.Add(new Vector3(0, 0, -1));

        obstacleMask = manager.GetComponent<FieldOfView>().obstacleMask;

        agent = manager.GetComponent<NavMeshAgent>();
    }
    public void Active()
    {
        //Walk towards player, when in range, fire arrows at set firespeed.
        //Arrow is its own object. when the arrow hits the player, it does damage to him/her. when the arrow hits an enemy, it does the same damage to the enemy. 
        //calculate angle & force of fire, so the arrow will always hit the player.
        //arrow has its own script, which arches the arrow in the direction its moving, and sends a message upward to the object it hits.
        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 8)
        {
            Move();
        }
        else if (distanceToTarget < 4)
        {
            MoveAway();
        }

        if (distanceToTarget < 7 && distanceToTarget > .5f)
        {
            InCombat();
        }
    }

    public void Move()
    {
        agent.speed = speed;
        agent.SetDestination(target.position);
    }

    public void MoveAway()
    {
        agent.speed = 0;
    }

    public void InCombat()
    {
        if (manager.GetComponent<AIManager>().visible == true)
        {
            Vector3 targetPos = target.position;
            targetPos.y = manager.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);
            manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);

            recalculatedPath = false;

            timer += 1 * Time.deltaTime;
            if (timer > fireRate && manager.GetComponent<AIManager>().dead != true)
            {
                manager.LookAt(target.position);
                Animator animator = manager.GetComponent<AIManager>().animator;
                animator.SetTrigger("Attack");
                timer = 0;
            }
        }

        if (recalculatedPath == false && manager.GetComponent<AIManager>().visible == false)
        {
            locations.Clear();

            latestPos = target.position;

            for (int i = 0; i < 4; i++)
            {
                if (!Physics.Raycast(latestPos, directions[i], 6, obstacleMask))
                {
                    locations.Add(latestPos + (directions[i] * 6));
                }
            }

            if (locations.Count <= 0)
            {
                agent.SetDestination(target.position);
                walkTarget = target.position;
                agent.speed = speed;
                recalculatedPath = true;
            }
            else
            {
                foreach (Vector3 loc in locations)
                {
                    float distance = Vector3.Distance(manager.position, loc);
                    distances.Add(distance);
                }

                for (int i = 0; i < distances.Count; i++)
                {
                    if (i == 0)
                    {
                        shortestDistance = distances[i];
                    }
                    if (distances[i] < shortestDistance)
                    {
                        shortestDistance = distances[i];
                    }
                }
                locationIndex = distances.IndexOf(shortestDistance);

                Vector3 targetPos = latestPos;
                targetPos.y = manager.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);
                manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 2 * Time.deltaTime);


                walkTarget = locations[locationIndex];
                agent.SetDestination(locations[locationIndex]);
                agent.speed = speed;

                distances.Clear();
                locations.Clear();

                recalculatedPath = true;
            }
        }
        if (recalculatedPath == true)
        {
            float distanceToTarget = Vector3.Distance(walkTarget, manager.position);
            if (distanceToTarget < 2)
            {
                recalculatedPath = false;
            }
        }
    }

    public void Attack()
    {
        manager.GetComponent<AIManager>().weapon.GetComponent<AssasinShuriken>().SpawnShuriken(manager.GetComponent<AIManager>().projectile, target.position);
    }
}
