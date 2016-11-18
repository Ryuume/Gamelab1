using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour {

	//TODO
    //Enemy will send signal when seeing player, when player is seen for 2-3 seconds, enemies will get suspicious, when enemies are suspicious, they will attack the player when they see him for 1 second.
    //When enemy is suspicious, they will call for help from nearby enemies, and check the last location of the player. When they spot the player, they will call out for help from enemies in a big radius.
    //when the player remains out of sight for 5 - 10 seconds, the enemy will decide it was "nothing" and continue its day. (only to probably be killed by the player later.. poor guy)

    public void EnemySpotted()
    {

    }
}
