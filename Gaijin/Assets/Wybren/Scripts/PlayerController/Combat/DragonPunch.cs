using UnityEngine;
using System.Collections;

public class DragonPunch : MonoBehaviour {

    float normalHeight;

    public GameObject player;
    public Transform model, handL, handR;

    public float jumpUp, jumpX = 7f, jumpZ = 7f;
    public bool straightMovement = false;

    void Start()
    {
        normalHeight = model.position.y;
    }

    void Begin()
    {
        player.GetComponent<PlayerController>().katana.SetActive(false);
        player.GetComponent<PlayerController>().freeze = true;
    }

    void Kneel()
    {
        //model.position = new Vector3(model.position.x, model.position.y - 0.3f, model.position.z);
    }

    void Stand()
    {

    }

    void Jump()
    {
        if (straightMovement == false)
        {
            if (player.GetComponent<PlayerController>().stateNum == 1)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(jumpX, jumpUp, -jumpZ);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 2)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(-jumpX, jumpUp, -jumpZ);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 3)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(-jumpX, jumpUp, jumpZ);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 4)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(jumpX, jumpUp, jumpZ);
            }
        }else
        {
            if (player.GetComponent<PlayerController>().stateNum == 1)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(jumpX, jumpUp, 0);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 2)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, jumpUp, -jumpZ);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 3)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(-jumpX, jumpUp, 0);
            }
            else if (player.GetComponent<PlayerController>().stateNum == 4)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, jumpUp, jumpZ);
            }
        }
    }

    void Ground(GameObject effect)
    {
        Instantiate(effect, new Vector3(model.position.x, 1, model.position.z), Quaternion.identity);
    }

    void ChargeStay(GameObject effect)
    {
        Instantiate(effect, handL.position, Quaternion.identity);
        Instantiate(effect, handR.position, Quaternion.identity);
    }

    void ChargeCircle(GameObject effect)
    {
        Instantiate(effect, new Vector3(model.position.x, 1, model.position.z), Quaternion.identity);
    }

    void Tatsu(GameObject effect)
    {
        Instantiate(effect, new Vector3(model.position.x, 1, model.position.z), model.rotation);
    }

    void End()
    {
        //player.GetComponent<PlayerController>().katana.transform.parent = player.GetComponent<PlayerController>().rHand;
        
        player.GetComponent<PlayerController>().freeze = false;
        player.GetComponent<PlayerController>().katana.SetActive(true);
    }
}
