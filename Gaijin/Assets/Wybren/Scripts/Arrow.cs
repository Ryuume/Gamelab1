using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
	void Update ()
    {
        transform.LookAt(transform.position, GetComponent<Rigidbody>().velocity);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == "Player")
        {
            print("HIT!");
        }
        if (col.transform.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
