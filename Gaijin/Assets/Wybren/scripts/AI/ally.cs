using UnityEngine;
using System.Collections;

public class Ally
{
    //TODO
    //1. Movement with animation
    //2. Have ally follow player (in a certain radius around the player.), target enemys that are in player range.
    //3. OPTIONAL: Make support class that doesnt attack enemys, but heals player when player health drops below certain level.
    //4. Have different attack types, so the ally can choose between enemys with lower / higher health/damage.
    //5. OPTIONAL: Have obstacle scaling like climbing and dropping down from a ledge.
    //6. Make Damage / Health / Speed adaptable by manager.

    public float speed, damage, health;

    public Transform manager;

    public bool inCombat = false;

    public UnitBehaviour unit;

    public Ally(Transform ai, float combatSpeed, float setDamage, float setHealth, UnitBehaviour behaviour)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        health = setHealth;
        unit = behaviour;
    }

    public void Active()
    {
        //TODO
        //Ranged unit runs towards target, when he gets in fire distance, he stops, and fires at the target. When target gets even closer to the archer, to a certain point he keeps shooting, and then pulls out a sword / dagger?
        //OPTIONAL: Ranged units try and find the higher ground, if this is available / in fire distance.
        //Archer shoots physics based arrows and a raycast, if raycast hits player, arrow gets fired. IF arrow hits player, damage is done, and arrow sticks around for 10 - 20 seconds.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.

        if (inCombat == true)
        {
            Combat();
        }

        else if (inCombat == false)
        {
            unit.AllyMove();
        }
    }

    public void Combat()
    {

    }
}
