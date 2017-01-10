using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    [HideInInspector]
    public float minDamage, maxDamage;

    public void RecieveData(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
    }

    void Update ()
    {
        transform.LookAt(transform.position, GetComponent<Rigidbody>().velocity);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            float damage = Random.Range(minDamage, maxDamage);
            //print("Arrow hit for " + damage + " damage");
            col.SendMessageUpwards("Hit", 8);
        }
        
        if (col.transform.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
