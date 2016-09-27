using UnityEngine;
using System.Collections;

public class Demon
{
    public float speed, damage, health;

    public Transform manager;

    public bool inCombat = false;

    public UnitBehaviour unit;

    public Demon(Transform ai, float combatSpeed, float setDamage, float setHealth, UnitBehaviour behaviour)
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
        //Ranged unit runs towards target, when he gets in fire distance, he stops, and fires at the target. When target gets even closer to the archer, to a certain point he keeps shooting, and then pulls out a sword / dagger, and moves towards the target.?
        //OPTIONAL: Ranged units try and find the higher ground, if this is available / in fire distance.
        //Archer shoots physics based arrows and a raycast, if raycast hits player, arrow gets fired. IF arrow hits player, damage is done, and arrow sticks around for 10 - 20 seconds.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.

        if (inCombat == true)
        {
            Combat();
        }

        else if (inCombat == false)
        {
            manager.GetComponent<AIManager>().StartAICoroutine(unit.Move());
        }
    }

    public void Combat()
    {

    }
}
