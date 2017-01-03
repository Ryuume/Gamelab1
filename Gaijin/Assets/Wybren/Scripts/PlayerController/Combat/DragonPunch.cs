using UnityEngine;
using System.Collections;

public class DragonPunch : MonoBehaviour {

    float normalHeight;

    public GameObject player;
    public Transform model;

    void Start()
    {
        normalHeight = model.position.y;
    }

    void Begin()
    {
        player.GetComponent<PlayerController>().katana.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;
    }

    void Kneel()
    {
        //model.position = new Vector3(model.position.x, model.position.y - 0.3f, model.position.z);
    }

    void Stand()
    {
        model.position = new Vector3(model.position.x, normalHeight, model.position.z);
    }

    void Jump()
    {
        if (player.GetComponent<PlayerController>().stateNum == 1)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(7, 12, -7);
        }
        else if (player.GetComponent<PlayerController>().stateNum == 2)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(-7, 12, -7);
        }else if (player.GetComponent<PlayerController>().stateNum == 3)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(-7, 12, 7);
        }
        else if (player.GetComponent<PlayerController>().stateNum == 4)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(7, 12, 7);
        }
    }

    void SpawnEffect()
    {

    }

    void End()
    {
        //player.GetComponent<PlayerController>().katana.transform.parent = player.GetComponent<PlayerController>().rHand;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerController>().katana.SetActive(true);
    }
}
