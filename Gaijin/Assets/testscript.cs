using UnityEngine;
using System.Collections;

public class testscript : MonoBehaviour {

    // Use this for initialization
    
    void Start ()
    {
        transform.rotation = Quaternion.Euler(0, 30, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Dinges();
        
    }
    void Dinges()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        if (t < 4)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / 4) % 360.0f;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
