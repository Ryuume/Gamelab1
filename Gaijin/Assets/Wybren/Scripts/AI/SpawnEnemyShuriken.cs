using UnityEngine;
using System.Collections;

public class SpawnEnemyShuriken : MonoBehaviour {

    public AIManager manager;

	public void SpawnShuriken()
    {
        manager.eUpdate.asUpdate.Attack();
    }
}
