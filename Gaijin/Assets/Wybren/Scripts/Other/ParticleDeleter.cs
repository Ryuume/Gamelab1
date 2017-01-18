using UnityEngine;
using System.Collections;

public class ParticleDeleter : MonoBehaviour {

    public float lifeDuration;
    float timer;
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer > lifeDuration)
        {
            Destroy(gameObject);
        }
	}
}
