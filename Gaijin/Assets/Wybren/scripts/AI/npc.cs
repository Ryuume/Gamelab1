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

    public float damage, health;

    public Transform manager;

    public bool loop, waiting, inCombat, ducking;

    public UnitBehaviour unit;

    public Npc(Transform ai, UnitBehaviour behaviour)
    {
        manager = ai;
        unit = behaviour;
    }

    public void Active()
    {
        //TODO
        //Move based on pathtype
        //react to player & allies with sound effect / animation / both
        //Run away from battle / duck and hide.
        if (inCombat == true)
        {
            unit.NpcRun();
        }

        else if (inCombat == false)
        {
            unit.ducking = false;
            manager.GetComponent<AIManager>().StartAICoroutine(unit.Move());
        }
    }
}
