using UnityEngine;
using System.Collections;

public class Kusarigama : MonoBehaviour
{
    public float minDamage, maxDamage;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            float damage = Random.Range(minDamage, maxDamage);
            print("Hit for " + damage + " damage");
            col.GetComponent<AIManager>().health -= damage;
        }
    }
}
