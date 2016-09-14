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

    [HideInInspector]
    public int attackPattern, pathType;

    public float speed, damage, health;

    public Transform path, manager;

    public void Update()
    {
        switch (manager.GetComponent<AIManager>().allyType)
        {
            case AIManager.AllyType.Ranged:
                {
                    Ranged();
                    break;
                }
            case AIManager.AllyType.Melee:
                {
                    Melee();
                    break;
                }
        }
    }

    public void GetStates()
    {
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

    public void Move()
    {
        //Move towards Target.
        //Stop when in range.
    }

    public void Targeter()
    {
        //Target enemys when in range based on attackPattern
        //When not in combar, Target next destination based player position.
        if (pathType == 0)//If pathtype = path
        {

        }
        else if (pathType == 1)//If pathType = stationary
        {

        }
    }

    public void Ranged()
    {
        //TODO
        //Ranged unit runs towards target, when he gets in fire distance, he stops, and fires at the target. When target gets even closer to the archer, to a certain point he keeps shooting, and then pulls out a sword / dagger?
        //OPTIONAL: Ranged units try and find the higher ground, if this is available / in fire distance.
        //Archer shoots physics based arrows and a raycast, if raycast hits player, arrow gets fired. IF arrow hits player, damage is done, and arrow sticks around for 10 - 20 seconds.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.

        Debug.Log("ally, " + "Ranged, " + attackPattern + ", " + pathType);
    }

    public void Melee()
    {
        //TODO
        //Melee unit runs towards target, and uses a set of attack animations when he gets withing range of the target.
        //Unit shoots a raycast and plays the animation when in range, if raycast is hit, damage to the target is done.
        //If attacked by another target, which is closer to the AI than the ai's target, switch target to the nearest attacking AI.

        Debug.Log("ally, " + "Melee, " + attackPattern + ", " + pathType);
    }
}
