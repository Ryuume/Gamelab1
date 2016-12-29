using UnityEngine;
using System.Collections;

public class Katana : MonoBehaviour {

    [HideInInspector]
    public bool doDamage;
    [HideInInspector]
    public float minDamage, maxDamage;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            if (doDamage == true)
            {
                float damage = Random.Range(minDamage, maxDamage);
                print("Hit for " + damage + " damage");
                col.GetComponent<AIManager>().health -= damage;
            }
        }
    }
}
