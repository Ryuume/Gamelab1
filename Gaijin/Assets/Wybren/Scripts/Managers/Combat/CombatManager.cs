using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

    //TODO
    //Enemy will send signal when seeing player, when player is seen for 2-3 seconds, enemies will get suspicious, when enemies are suspicious, they will attack the player when they see him for 1 second.
    //When enemy is suspicious, they will call for help from nearby enemies, and check the last location of the player. When they spot the player, they will call out for help from enemies in a big radius.
    //when the player remains out of sight for 5 - 10 seconds, the enemy will decide it was "nothing" and continue its day. (only to probably be killed by the player later.. poor guy)

    //Maintain list of all enemies after player when in combat, if all enemies are dead, set player to non combat. 
    //music will also use this script to determine which music to play.

    public PlayerController player;
    public List<AIManager> inCombatAI = new List<AIManager>();
    public bool combat;

    public void Update()
    {
        if(inCombatAI.Count == 0)
        {
            combat = false;
            player.inCombat = false;
        }
        else
        {
            combat = true;
        }
    }

    public void Suspicious(List<AIManager> enemiesInRadius, Vector3 target)
    {
        //list contains all enemies in the small help radius.
        //set all enemies in list to suspicious, and give target of original AI that spotted player.
        foreach (AIManager _AI in enemiesInRadius)
        {
            _AI.suspicious = true;
            _AI.eUpdate.sTarget = target;
            _AI.eUpdate.unit.target = null;
            _AI.eUpdate.unit.randomTarget = Vector3.zero;
        }
    }

    public void SpottedPlayer(List<AIManager> enemiesInRadius, Transform target)
    {
        //list contains all enemies in the large help radius.
        //set all enemies in list to InCombat, and give target of original AI that spotted player.
        foreach(AIManager _AI in enemiesInRadius)
        {
            _AI.inCombat = true;
            _AI.eUpdate.target = target;
            _AI.gameObject.GetComponent<FieldOfView>().draw = false;
            _AI.gameObject.GetComponent<AreaOfView>().draw = false;
            inCombatAI.Add(_AI);
        }
        if(player.wielding == false)
        {
            player.animator.SetTrigger("Draw");
        }
        player.wielding = true;
        player.inCombat = true;
    }

    public void EnemyDeath(AIManager _AI)
    {
        inCombatAI.Remove(_AI);
    }
}
