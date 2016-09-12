using UnityEngine;
using System.Collections;

public class area : MonoBehaviour {

	public Vector3 point (float walkRadius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        return randomDirection;
    }
}
