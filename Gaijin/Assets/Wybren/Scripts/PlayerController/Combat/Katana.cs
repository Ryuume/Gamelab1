using UnityEngine;
using System.Collections;

public class Katana : MonoBehaviour {


    public bool doDamage, isEnemy;
    [HideInInspector]
    public float minDamage, maxDamage;

    public void RecieveData(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
        isEnemy = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isEnemy == false)
        {
            if (col.gameObject.tag == "Enemy")
            {
                if (doDamage == true)
                {
                    float damage = Random.Range(minDamage, maxDamage);
                    print("Hit for " + damage + " damage");
                    col.SendMessageUpwards("Hit", damage);
                }
            }
        }else
        {
            if (col.gameObject.tag == "Player")
            {
                if (doDamage == true)
                {
                    float damage = Random.Range(minDamage, maxDamage);
                    print("Enemy hit for " + damage + " damage");
                    doDamage = false;
                    col.SendMessageUpwards("Hit", damage);
                }
            }
        }
    }
}
