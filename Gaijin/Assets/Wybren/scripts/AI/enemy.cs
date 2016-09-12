using UnityEngine;
using System.Collections.Generic;

public class enemy
{
    //TODO
    //1. Movement with animation
    //2. Have enemy attack player when player is in range, or player allys depending on attack type.
    //3. OPTIONAL: Make support class that doesnt attack player, but heals enemy when enemy health drops below certain level.
    //4. Have different attack types, so the enemy can choose between player / ally with lower / higher health/damage.
    //5. OPTIONAL: Have obstacle scaling like climbing and dropping down from a ledge.
    //6. Have different types of pathfinding (wander, stationary until player in range, or with a path made by UX Designer)
    //7. make speed, health, damage and all that adaptable so the manager can set them.

    [HideInInspector]
    public int attackPattern, pathType, aIType, indexer = 0, editor = 0;

    public float speed, damage, health;

    public Transform path, manager, target;

    public bool inCombat = false, loop;

    public List<Transform> waypoints = new List<Transform>();

    public void Update()
    {
        switch (manager.GetComponent<aiManager>().enemyType)
        {
            case aiManager.EnemyType.Ranged:
                {
                    Ranged();
                    break;
                }
            case aiManager.EnemyType.Melee:
                {
                    Melee();
                    break;
                }
        }
    }

    public void GetStates()
    {
        switch (manager.GetComponent<aiManager>().priority)
        {
            case aiManager.Priority.HighestDamage:
                {
                    attackPattern = 0;
                    break;
                }
            case aiManager.Priority.LowestDamage:
                {
                    attackPattern = 1;
                    break;
                }
            case aiManager.Priority.HighestHealth:
                {
                    attackPattern = 2;
                    break;
                }
            case aiManager.Priority.LowestHealth:
                {
                    attackPattern = 3;
                    break;
                }
        }

        switch (manager.GetComponent<aiManager>().pathType)
        {
            case aiManager.PathType.Path:
                {
                    pathType = 0;
                    break;
                }
            case aiManager.PathType.Stationary:
                {
                    pathType = 1;
                    break;
                }
        }
        path = manager.GetComponent<aiManager>().path;

        foreach (Transform child in path)
        {
            waypoints.Add(child);
        }

        NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Move()
    {

    }

    public void Targeter()
    {
        if (inCombat == false)
        {
            if (pathType == 0)//If pathtype = path
            {
                if (target == null)
                {
                    target = waypoints[0];
                    NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                    agent.SetDestination(target.position);
                }

                float distanceToTarget = Vector3.Distance(manager.position, target.position);

                if (distanceToTarget < 1.5)
                {
                    if (indexer == 0)
                    {
                        editor = 1;
                    }
                    else if (indexer == waypoints.Count - 1 && loop == false)
                    {
                        editor = -1;
                    }
                    else if (indexer == waypoints.Count - 1 && loop == true)
                    {
                        indexer = -1;
                    }

                    indexer = indexer + editor;
                    target = waypoints[indexer];
                    NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                    agent.SetDestination(target.position);
                }
            }
        }
        else
        {
            //make a targeter that targets nearby enemys.

            if (attackPattern == 0) //If attackpattern = HighestDamage
            {

            }
            else if (attackPattern == 1) //If attackpattern = LowestDamage
            {

            }
            else if (attackPattern == 2) //If attackpattern = HighestHealth
            {

            }
            else if (attackPattern == 3) //If attackpattern = LowestHealth
            {

            }
        }
    }

    public void Ranged()
    {
        //TODO
        //Ranged unit runs towards target, when he gets in fire distance, he stops, and fires at the target. When target gets even closer to the archer, to a certain point he keeps shooting, and then pulls out a sword / dagger, and moves towards the target.?
        //OPTIONAL: Ranged units try and find the higher ground, if this is available / in fire distance.
        //Archer shoots physics based arrows and a raycast, if raycast hits player, arrow gets fired. IF arrow hits player, damage is done, and arrow sticks around for 10 - 20 seconds.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.


        Targeter();
    }

    public void Melee()
    {
        //TODO
        //Melee unit runs towards target, and uses a set of attack animations when he gets withing range of the target.
        //Unit shoots a raycast and plays the animation when in range, if raycast is hit, damage to the target is done.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.


        Targeter();
    }
}
