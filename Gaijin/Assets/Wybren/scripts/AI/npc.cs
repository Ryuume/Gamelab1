﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class npc
{
    //TODO
    //1. Movement with animations
    //2. Have movement with: Wander / Stationary / prebuild Path
    //3. Have different types of npc: (Generic, Shop, Dialogue / quest giver?)
    //4. Make speed, health adaptable by manager.

    [HideInInspector]
    public int pathType, indexer = 0, editor = 0;

    public float speed, damage, health;

    public Transform path, manager, target;

    public List<Transform> waypoints = new List<Transform>();

    public bool loop, waiting;

    public void GetStates()
    {
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

        foreach (Transform child in path)
        {
            waypoints.Add(child);
        }

        NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Move ()
    {
        
    }

    public IEnumerator Targeter()
    {
        //pick next target based on pathType.
        //pick random far position when near combat, when this fails, duck and hide
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
                if(target.GetComponent<node>().waitTime != 0.0)
                {
                    Debug.Log("Waiting for " + target.GetComponent<node>().waitTime + " seconds Sir!");
                    waiting = true;
                    yield return new WaitForSeconds(target.GetComponent<node>().waitTime);
                    waiting = false;
                    Debug.Log("Moving sir!");
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
        }
    }

    public void Generic()
    {
        //TODO
        //Move based on pathtype
        //react to player & allies with sound effect / animation / both
        //Run away from battle / duck and hide.
        if (waiting == false)
        {
            manager.GetComponent<aiManager>().StartAICoroutine(Targeter());
        }
;    }
}
