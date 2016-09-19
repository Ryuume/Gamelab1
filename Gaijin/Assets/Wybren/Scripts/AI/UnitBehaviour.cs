using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehaviour
{
    //All Variables for movement.
    public int indexer = 0, editor = 0;
    public float walkRadius;
    public Transform manager, target;
    public List<Transform> waypoints = new List<Transform>();
    public bool loop, waiting;
    public Vector3 randomTarget, randomDirection;

    //All Variables for attacking.
    public int attackPattern;

    public virtual IEnumerator Move()
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
                            if (target.GetComponent<Node>().waitTime != 0.0)
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

    public virtual void Targeter()
    {
        //make a targeter that targets nearby enemys.
        switch (manager.GetComponent<AIManager>().priority)
        {
            case AIManager.Priority.HighestDamage:
                {
                    attackPattern = 0;
                    break;
                }
            case AIManager.Priority.LowestDamage:
                {
                    attackPattern = 1;
                    break;
                }
            case AIManager.Priority.HighestHealth:
                {
                    attackPattern = 2;
                    break;
                }
            case AIManager.Priority.LowestHealth:
                {
                    attackPattern = 3;
                    break;
                }
        }
    }
}
