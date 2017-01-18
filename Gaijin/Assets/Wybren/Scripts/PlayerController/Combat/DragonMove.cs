using UnityEngine;
using System.Collections;

public class DragonMove : MonoBehaviour {

    public float speed;
    public float lifeDuration;
    float timer;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player")
        {
            col.SendMessageUpwards("Hit", 200);
        }
    }
}
