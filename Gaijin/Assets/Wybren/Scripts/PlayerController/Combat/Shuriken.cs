using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask targetMask;
    Vector3 mousePos;

    public float speed;
    float t;

    float minDamage, maxDamage;

    bool isEnemy;
    [HideInInspector]
    public Vector3 target;

    public void PlaySound()
    {

    }


    void SetDamage(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
    }

    void RecieveData(Vector3 _Target)
    {
        target = _Target;
        isEnemy = true;
    }

    void Start()
    {
        if (isEnemy == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
            {
                Vector3 targetPos = hit.point;
                if (Vector3.Distance(transform.position, targetPos) > .5f)
                {
                    targetPos.y = transform.position.y;
                    mousePos = targetPos;
                }
            }

            Vector3 dir = (mousePos - transform.position);

            GetComponent<Rigidbody>().velocity = dir * speed;
        }else
        {
            Vector3 dir = (target - transform.position);
            GetComponent<Rigidbody>().velocity = dir * 10;
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > 5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (isEnemy == false)
        {
            if (col.gameObject.tag == "Enemy")
            {
                float damage = Random.Range(10, 20);
                print("Hit for " + damage + " damage");
                col.SendMessageUpwards("Hit", damage);
            }

            if (col.gameObject.tag != "Player")
            {
                Destroy(gameObject);
            }
        }else
        {
            if (col.gameObject.tag == "Player")
            {
                float damage = Random.Range(minDamage, maxDamage);
                print("Hit for " + damage + " damage");
                col.SendMessageUpwards("Hit", damage);
            }

            if (col.gameObject.tag != "Enemy")
            {
                Destroy(gameObject);
            }
        }
    }

}
