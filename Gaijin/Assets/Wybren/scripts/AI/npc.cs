using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Npc
{
    //TODO
    //1. Movement with animations
    //2. Have movement with: Wander / Stationary / prebuild Path
    //3. Have different types of npc: (Generic, Shop, Dialogue / quest giver?)
    //4. Make speed, health adaptable by manager.

    [HideInInspector]
    public int pathType, indexer = 0, editor = 0;

    public float speed, damage, health, walkRadius;

    public Transform path, manager, target;

    public List<Transform> waypoints = new List<Transform>();

    public bool loop, waiting, inCombat, ducking;

    public Vector3 randomTarget, randomDirection;

    public void GetStates()
    {
        foreach (Transform child in path)
        {
            waypoints.Add(child);
        }

        NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    

    public void Targeter()
    {
        //pick next target based on pathType.
        //pick random far position when near combat, when this fails, duck and hide

            //randomize between running, or ducking out of fear.
            int randomNumber = Random.Range(0, 1);

            if(randomNumber == 0)
            {
                //run
                ducking = true;
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 direction = player.position - manager.position;
                direction = -direction;

                Vector3 runTarget = direction * Random.Range(5, 10);

                NavMeshHit hit;
                NavMesh.SamplePosition(runTarget, out hit, walkRadius, 1);
                runTarget = hit.position;

                NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                agent.SetDestination(runTarget);
                target = null;
                randomTarget = Vector3.zero;
            }
            else if (randomNumber == 1)
            {
                //duck & hide
                ducking = true;
                NavMeshAgent agent = manager.GetComponent<NavMeshAgent>();
                agent.destination = manager.position;
                Debug.Log("HAAAAAAAAAAAALLLPP! *ducks*");
                target = null;
                randomTarget = Vector3.zero;
                //call animation * sound effect.
            }
    }

    public void Generic()
    {
        //TODO
        //Move based on pathtype
        //react to player & allies with sound effect / animation / both
        //Run away from battle / duck and hide.
        if (inCombat == true)
        {
            Targeter();
        }

        if (inCombat == false)
        {
            ducking = false;
        }

        if (waiting == false && ducking == false)
        {
            //manager.GetComponent<AIManager>().StartAICoroutine(Move());
        }
    }
}
