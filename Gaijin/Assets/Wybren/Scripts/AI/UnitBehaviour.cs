using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehaviour
{
    //All Variables for movement.
    public int indexer = 0, editor = 0;
    public float walkRadius, speed;
    public Transform manager, target;
    public List<Transform> waypoints = new List<Transform>();
    public bool loop, waiting, ducking;
    public Vector3 randomTarget, randomDirection, searchTarget, searchDirection;

    //All Variables for attacking.
    public int attackPattern;

    public UnitBehaviour(float walkR, float aiSpeed, Transform ai, Transform path, bool loopPath)
    {
        walkRadius = walkR;
        manager = ai;

        foreach(Transform child in path)
        {
            waypoints.Add(child);
        }

        speed = aiSpeed;
        loop = loopPath;
    }

    public IEnumerator Move()
    {
        if (waiting == false)
        {
            switch (manager.GetComponent<AIManager>().pathType)
            {
                //Makes following a path possible.
                case AIManager.PathType.Path:
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
                            if (target.GetComponent<Node>().waitTime != 0.0 && target.GetComponent<Node>() != null)
                            {
                                waiting = true;
                                yield return new WaitForSeconds(target.GetComponent<Node>().waitTime);
                                waiting = false;
                            }

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
                        break;
                    }
                
                //Makes standing still possible.
                case AIManager.PathType.Stationary:
                    {
                        //play stationary animation(s);
                        break;
                    }

                //Makes walking a random direction possible.
                case AIManager.PathType.Wander:
                    {
                        if (manager.GetComponent<AIManager>().wanderInArea == false)
                        {
                            randomDirection = Random.insideUnitSphere * walkRadius;
                        }
                        else
                        {
                            randomDirection = manager.GetComponent<AIManager>().wanderArea.GetComponent<Area>().point(walkRadius);
                        }

                        if (randomTarget == Vector3.zero)
                        {
                            randomDirection += manager.position;
                            NavMeshHit hit;
                            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                            randomTarget = hit.position;

                            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                            agent.SetDestination(randomTarget);
                        }

                        float distanceToTarget = Vector3.Distance(manager.position, randomTarget);

                        if (distanceToTarget < 1.5)
                        {
                            randomDirection += manager.position;
                            NavMeshHit hit;
                            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                            randomTarget = hit.position;
                            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                            agent.SetDestination(randomTarget);
                        }
                        break;
                    }
            }
        }
    }

    public void AllyMove()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        float distanceToPlayer = Vector3.Distance(manager.position, player.position);

        if (distanceToPlayer > 4)
        {
            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
            agent.speed = speed;
            agent.SetDestination(player.position);
        }
        else
        {
            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
            agent.speed = 0;
        }
    }

    public void NpcRun()
    {
        //pick next target based on pathType.
        //pick random far position when near combat, when this fails, duck and hide
        if (ducking == false)
        {         //randomize between running, or ducking out of fear.
            int randomNumber = Random.Range(0, 2);

            if (randomNumber == 0 || randomNumber == 1)
            {
                //run
                ducking = true;
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 direction = player.position - manager.position;
                direction = -direction;

                Debug.Log("run");

                Vector3 runTarget = direction * Random.Range(5, 10);

                NavMeshHit hit;
                NavMesh.SamplePosition(runTarget, out hit, walkRadius, 1);
                runTarget = hit.position;

                NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                agent.SetDestination(runTarget);
                target = null;
                randomTarget = Vector3.zero;
            }
            else if (randomNumber == 2)
            {
                ducking = true;
                //duck & hide
                NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                agent.destination = manager.position;
                Debug.Log("HAAAAAAAAAAAALLLPP! *ducks*");
                target = null;
                randomTarget = Vector3.zero;
                //call animation * sound effect.
            }
        }
    }

    public void SearchArea(Vector3 searchArea)
    {
        if (searchTarget == Vector3.zero)
        {
            searchDirection = Random.insideUnitSphere * walkRadius / 2;
            searchDirection = searchDirection + searchArea;

            searchDirection += manager.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(searchDirection, out hit, 5, 1);
            searchTarget = hit.position;

            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
            agent.SetDestination(searchTarget);
        }

        float distanceToTarget = Vector3.Distance(manager.position, searchTarget);

        if (distanceToTarget < 2)
        {
            searchDirection = Random.insideUnitSphere * walkRadius / 2;
            searchDirection = searchDirection + searchArea;

            searchDirection += manager.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(searchDirection, out hit, 5, 1);
            searchTarget = hit.position;

            NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
            agent.SetDestination(searchTarget);
        }
    }
}
