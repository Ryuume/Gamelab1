using UnityEngine;
using System.Collections;

public class Dagger : MonoBehaviour
{
    public bool doDamage, isEnemy;
    [HideInInspector]
    public float minDamage, maxDamage;

    void DoDamage()
    {
        doDamage = true;
    }
    void NoDamage()
    {
        doDamage = false;
    }

    public void RecieveData(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")
        {
            if (doDamage == true)
            {
                float damage = Random.Range(minDamage, maxDamage);
                print("Hit for " + damage + " damage");
                col.SendMessageUpwards("Hit", damage);
                doDamage = false;
            }
        }
    }
}
