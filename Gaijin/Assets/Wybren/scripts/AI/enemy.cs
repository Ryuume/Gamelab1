using UnityEngine;
using System.Collections.Generic;

public class Enemy
{
    //TODO
    //1. Movement with animation
    //2. Have enemy attack player when player is in range, or player allys depending on attack type.
    //3. OPTIONAL: Make support class that doesnt attack player, but heals enemy when enemy health drops below certain level.
    //4. Have different attack types, so the enemy can choose between player / ally with lower / higher health/damage.
    //5. OPTIONAL: Have obstacle scaling like climbing and dropping down from a ledge.
    //6. Have different types of pathfinding (wander, stationary until player in range, or with a path made by UX Designer)
    //7. make speed, health, damage and all that adaptable so the manager can set them.


    public Transform manager, target;

    public Vector3 sTarget;

    public bool inCombat = false, visible = false, suspicious = false;

    public UnitBehaviour unit;

    Guard gUpdate;
    Archer aUpdate;
    Scout sUpdate;
    Assasin asUpdate;
    Samurai saUpdate;

    public Enemy(Transform ai, float combatSpeed, float setDamage, float setHealth, UnitBehaviour behaviour)
    {
        manager = ai;
        unit = behaviour;

        switch (manager.GetComponent<AIManager>().enemyType)
        {
            case AIManager.EnemyType.Guard:
                {
                    gUpdate = new Guard(ai, combatSpeed, setDamage, setHealth);
                    break;
                }
            case AIManager.EnemyType.Archer:
                {
                    aUpdate = new Archer(ai, combatSpeed, setDamage, setHealth);
                    break;
                }
            case AIManager.EnemyType.Scout:
                {
                    sUpdate = new Scout(ai, combatSpeed, setDamage, setHealth);
                    break;
                }
            case AIManager.EnemyType.Assasin:
                {
                    asUpdate = new Assasin(ai, combatSpeed, setDamage, setHealth);
                    break;
                }
            case AIManager.EnemyType.Samurai:
                {
                    saUpdate = new Samurai(ai, combatSpeed, setDamage, setHealth);
                    break;
                }
        }
    }

    public void Active()
    {
        //TODO
        //Ranged unit runs towards target, when he gets in fire distance, he stops, and fires at the target. When target gets even closer to the archer, to a certain point he keeps shooting, and then pulls out a sword / dagger, and moves towards the target.?
        //OPTIONAL: Ranged units try and find the higher ground, if this is available / in fire distance.
        //Archer shoots physics based arrows and a raycast, if raycast hits player, arrow gets fired. IF arrow hits player, damage is done, and arrow sticks around for 10 - 20 seconds.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.

        if (inCombat == true)
        {
            Combat();
        }
        else if (inCombat == false && visible == false && suspicious == false)
        {
            manager.GetComponent<AIManager>().StartAICoroutine(unit.Move());
        }

        if (suspicious == true && inCombat == false)
        {
            Suspicious();
        }
    }

    public void Suspicious()
    {
        //walk to target position (given by AIManager when suspicious is set to true)
        manager.GetComponent<NavMeshAgent>().SetDestination(sTarget);
        manager.GetComponent<NavMeshAgent>().speed = manager.GetComponent<AIManager>().speed;
    }

    public void Combat()
    {
        switch(manager.GetComponent<AIManager>().enemyType)
        {
            case AIManager.EnemyType.Guard:
                {
                    gUpdate.Active();
                    gUpdate.target = target;
                    break;
                }
            case AIManager.EnemyType.Archer:
                {
                    aUpdate.Active();
                    aUpdate.target = target;
                    break; 
                }
            case AIManager.EnemyType.Scout:
                {
                    sUpdate.Active();
                    sUpdate.target = target;
                    break;
                }
            case AIManager.EnemyType.Assasin:
                {
                    asUpdate.Active();
                    asUpdate.target = target;
                    break;
                }
            case AIManager.EnemyType.Samurai:
                {
                    saUpdate.Active();
                    saUpdate.target = target;
                    break;
                }
        }
    }
}
