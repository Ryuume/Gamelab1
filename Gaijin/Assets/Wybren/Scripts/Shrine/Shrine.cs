using UnityEngine;
using System.Collections;

public class Shrine : MonoBehaviour
{
    public GameObject usePopup, particle;
    bool used = false;



    public void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if (col.GetComponent<PlayerController>() != null)
            {
                if (col.GetComponent<PlayerController>().health < 100)
                {
                    usePopup.SetActive(true);
                    if (Input.GetButtonDown("Use") && used == false)
                    {
                        print("use");
                        usePopup.SetActive(false);
                        used = true;
                        particle.SetActive(false);
                        col.GetComponent<PlayerController>().health = 100;
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            usePopup.SetActive(false);
        }
    }
}
