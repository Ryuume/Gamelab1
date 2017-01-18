using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
    void SetDeath()
    {
        prefab.GetComponent<Rigidbody>().isKinematic = false;
        prefab.GetComponent<CapsuleCollider>().radius = .2f;
        prefab.GetComponent<CapsuleCollider>().height = .1f;
    }
    void Delete()
    {
        prefab.GetComponent<AIManager>().enabled = false;
    }
}
