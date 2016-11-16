using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //TODO
    //1. Get mouse position in worldspace. +++
    //2. Calculate at what angle the mouse is compared to the player.
    //3. Have four states of movement, which call different animations (movement stays the same).
    //4. Switch through states based on 2 and call necessary animations.
    //5. Call all necessary animations.
    //6. Have health, and such as variables.
    //7. Register hits, death and such and play animations based on this.

    RaycastHit hit;

    public LayerMask targetMask;
    public Transform camera;

    Vector3 mousePos, top, bottom, left, right;
    enum lookState { top, bottom, left, right }
    lookState currentState;

    public void Start()
    {
        currentState = lookState.top;

        top = camera.forward;
        bottom = -camera.forward;
        left = -camera.right;
        right = camera.right;
    }

    public void Update()
    {
        lookStates();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(transform.position, top, Color.red);
        Debug.DrawRay(transform.position, bottom, Color.red);
        Debug.DrawRay(transform.position, left, Color.green);
        Debug.DrawRay(transform.position, right, Color.green);

        Debug.DrawRay(transform.position, new Vector3(1, 0, 0), Color.white);
        Debug.DrawRay(transform.position, new Vector3(-1, 0, 0), Color.white);
        Debug.DrawRay(transform.position, new Vector3(0, 0, 1), Color.white);
        Debug.DrawRay(transform.position, new Vector3(0, 0, -1), Color.white);



        if (Physics.Raycast(ray, out hit, targetMask))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;
            Debug.DrawLine(transform.position, targetPos, Color.blue);
            if (Vector3.Distance(transform.position, targetPos) > .5f)
            {
                mousePos = targetPos;
            }
        }
    }

    public void lookStates()
    {
        switch (currentState)
        {
            case lookState.top:
                {
                    State1();
                    break;
                }
            case lookState.bottom:
                {
                    State3();
                    break;
                }
            case lookState.left:
                {
                    State4();
                    break;
                }
            case lookState.right:
                {
                    State2();
                    break;
                }
        }

        
    }

    void State1()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(new Vector3(0, 0, 1), targetDir);
        if(mousePos.x > transform.position.x)
        {
           // print("1");
        }else
        {
            angle = (-angle);
        }
        print(angle);

        if (angle > 45)
        {
            currentState = lookState.right;
        }else if (angle < -45)
        {
            currentState = lookState.left;
        }
    }

    void State2()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(new Vector3(1, 0, 0), targetDir);
        if (mousePos.z < transform.position.z)
        {
            //print("2");
        }else
        {
            angle = (-angle);
        }
        print(angle);

        if (angle > 45)
        {
            currentState = lookState.bottom;
        }else if (angle < -45)
        {
            currentState = lookState.top;
        }
    }

    void State3()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(new Vector3(0, 0, -1), targetDir);
        if (mousePos.x < transform.position.x)
        {
           // print("3");
        }else
        {
            angle = (-angle);
        }
        print(angle);

        if (angle > 45)
        {
            currentState = lookState.left;
        }else if (angle < -45)
        {
            currentState = lookState.right;
        }
    }
    void State4()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(new Vector3(-1, 0, 0), targetDir);
        if (mousePos.z > transform.position.z)
        {
           //print("4");
        }else
        {
            angle = (-angle);
        }
        print(angle);

        if (angle > 45)
        {
            currentState = lookState.top;
        }else if (angle < -45)
        {
            currentState = lookState.bottom;
        }
    }
}
