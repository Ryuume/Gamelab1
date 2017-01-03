using UnityEngine;
using System.Collections;

public class SmokeBomb : MonoBehaviour
{
    float t;

    void Update()
    {
        t += Time.deltaTime;
        if(t > 1f)
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.SendMessageUpwards("Stun");
        }
    }
}
