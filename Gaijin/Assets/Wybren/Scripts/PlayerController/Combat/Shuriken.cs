using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask targetMask;
    Vector3 mousePos;

    public float speed;

    void Start()
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
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            float damage = Random.Range(10, 20);
            print("Hit for " + damage + " damage");
            col.gameObject.GetComponent<AIManager>().health -= damage;
        }

        if (col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

}
