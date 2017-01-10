using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Archer
{
    public float speed, damage, fireRate, health, timer, shortestDistance, projectileSpeed = 20f;

    int locationIndex;

    public Transform manager, target;

    public List<Vector3> directions = new List<Vector3>();

    public bool recalculatedPath, directFire = true;

    Vector3 latestPos, walkTarget;

    NavMeshAgent agent;

    LayerMask obstacleMask;

    public Archer(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
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

        if (distanceToTarget > 10 && recalculatedPath == false)
        {
            Move();
        }else if (distanceToTarget < 4 && recalculatedPath == false)
        {
            MoveAway();
        }

        if (distanceToTarget < 13 && distanceToTarget > 3)
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
            if (timer > fireRate)
            {
                Attack();
                timer = 0;
            }
        }

        if (recalculatedPath == false && manager.GetComponent<AIManager>().visible == false)
        {
            walkTarget = RecalculatePath();
            agent.SetDestination(walkTarget);
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

    public Vector3 RecalculatePath()
    {
        List<float> distances = new List<float>();
        List<Vector3> locations = new List<Vector3>();

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
            return target.position;
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

            return locations[locationIndex];
        }
    }

    public void Attack()
    {
        GameObject projectile = manager.GetComponent<AIManager>().weapon;
        Vector3 targetHit = target.position;
        Vector3 source = manager.position;

        
        float x = (targetHit - source).magnitude;
        float y = targetHit.y + x/4;
        float v = projectileSpeed * x/4;
        float g = Physics.gravity.y;
        float v2 = v * v;
        float v4 = v2 * v2;
        float fac = v4 - g * (g * x * x + 2 * y * v2);
        if (fac < 0)
        {
            Debug.Log("No valid angle");
        }
        float theta = -Mathf.Atan((v2 - Mathf.Sqrt(fac)) / (g * x)) * Mathf.Rad2Deg;
        while (theta < 0) theta += 362f;

        

        GameObject arrow = (GameObject)MonoBehaviour.Instantiate(projectile, manager.position, Quaternion.identity);
        float distance = Vector3.Distance(manager.position, target.position);
        Vector3 targetPos = target.position + (target.GetComponent<Rigidbody>().velocity / (distance / 3));
        targetPos.y = manager.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);
        arrow.transform.rotation = targetRotation;
        arrow.transform.Rotate(theta, 0, 0);
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * projectileSpeed;
    }
}
