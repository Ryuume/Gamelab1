using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

	public Vector3 point (float walkRadius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        return randomDirection;
    }
}
