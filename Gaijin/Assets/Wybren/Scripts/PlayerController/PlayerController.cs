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
    public Transform refDir, feet, head;

    Vector3 mousePos, top, bottom, left, right;
    enum lookState { top, bottom, left, right }
    lookState currentState;

    public float speed, length;
    public float xSpeed, zSpeed;
    public Animator animator;

    public void Start()
    {
        currentState = lookState.top;

        top = refDir.forward;
        bottom = -refDir.forward;
        left = -refDir.right;
        right = refDir.right;
    }

    public void Update()
    {
        Move();
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



        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = head.position.y;
            Debug.DrawLine(transform.position, targetPos, Color.blue);
            if (Vector3.Distance(transform.position, targetPos) > .5f)
            { 
                targetPos.y = head.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(targetPos - head.position);
                head.rotation = Quaternion.Slerp(head.rotation, targetRotation, 4 * Time.deltaTime);
                targetPos.y = feet.position.y;
                mousePos = targetPos;
            }
        }
    }

    void Move()
    {
        if (Physics.Raycast(transform.position, right, length) == false)
        {
            if (Input.GetButton("Right"))
            {

                //transform.Translate(right * speed * Time.deltaTime);
                if (xSpeed < 1f)
                {
                    xSpeed += 0.1f;
                }
            }
        }

        //Checkt of er muren aan de linkerkant van de speler zitten, zoniet mag de speler lopen.
        if (Physics.Raycast(transform.position, left, length) == false)
        {
            if (Input.GetButton("Left"))
            {

                //transform.Translate(left * speed * Time.deltaTime);
                if (xSpeed > -1f)
                {
                    xSpeed -= 0.1f;
                }
            }
        }

        //Checkt of er muren voor de de speler zitten, zoniet mag de speler lopen.
        if (Physics.Raycast(transform.position, top, length) == false)
        {
            if (Input.GetButton("Up"))
            {

                //transform.Translate(top * speed * Time.deltaTime);
                if (zSpeed < 1f)
                {
                    zSpeed += 0.1f;
                }
            }
        }

        //Checkt of er muren achter de speler zitten, zoniet mag de speler lopen.
        if (Physics.Raycast(transform.position, bottom, length) == false)
        {
            if (Input.GetButton("Down"))
            {

                //transform.Translate(bottom * speed * Time.deltaTime);
                if (zSpeed > -1f)
                {
                    zSpeed -= 0.1f;
                }
            }
        }

        if (!Input.GetButton("Right") && !Input.GetButton("Left"))
        {
            if(xSpeed > 0f)
            {
                xSpeed -= 0.1f;
            }else if (xSpeed < 0f)
            {
                xSpeed += 0.1f;
            }
            if(xSpeed < 0.1f && xSpeed > -0.1f)
            {
                xSpeed = 0;
            }
        }
        if (!Input.GetButton("Up") && !Input.GetButton("Down"))
        {
            if (zSpeed > 0f)
            {
                zSpeed -= 0.1f;
            }else if (zSpeed < 0f)
            {
                zSpeed += 0.1f;
            }
            if (zSpeed < 0.1f && zSpeed > -0.1f)
            {
                zSpeed = 0;
            }
        }

       

        transform.Translate((speed * xSpeed * Time.deltaTime), 0, (speed * zSpeed * Time.deltaTime));
        //transform.GetComponent<Rigidbody>().velocity = new Vector3((speed * xSpeed * Time.deltaTime), 0, (speed * zSpeed * Time.deltaTime));
    }

    public void lookStates()
    {
        switch (currentState)
        {
            case lookState.top:
                {
                    State1();
                    SwitchState1();
                    break;
                }
            case lookState.bottom:
                {
                    State3();
                    SwitchState3();
                    break;
                }
            case lookState.left:
                {
                    State4();
                    SwitchState4();
                    break;
                }
            case lookState.right:
                {
                    State2();
                    SwitchState2();
                    break;
                }
        }

        
    }

    void SwitchState1()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(top, targetDir);
        if(mousePos.x > transform.position.x)
        {
           // print("1");
        }else
        {
            angle = (-angle);
        }

        if (angle > 45)
        {
            currentState = lookState.right;
        }else if (angle < -45)
        {
            currentState = lookState.left;
        }
    }
    void SwitchState2()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(right, targetDir);
        if (mousePos.z < transform.position.z)
        {
            //print("2");
        }else
        {
            angle = (-angle);
        }

        if (angle > 45)
        {
            currentState = lookState.bottom;
        }else if (angle < -45)
        {
            currentState = lookState.top;
        }
    }
    void SwitchState3()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(bottom, targetDir);
        if (mousePos.x < transform.position.x)
        {
           // print("3");
        }else
        {
            angle = (-angle);
        }

        if (angle > 45)
        {
            currentState = lookState.left;
        }else if (angle < -45)
        {
            currentState = lookState.right;
        }
    }
    void SwitchState4()
    {
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(left, targetDir);
        if (mousePos.z > transform.position.z)
        {
           //print("4");
        }else
        {
            angle = (-angle);
        }

        if (angle > 45)
        {
            currentState = lookState.top;
        }else if (angle < -45)
        {
            currentState = lookState.bottom;
        }
    }

    void State1()
    {
        feet.transform.eulerAngles = new Vector3(0, 45, 0);

        animator.SetFloat("PosZ", zSpeed);
        animator.SetFloat("PosX", xSpeed);
    }
    void State2()
    {
        feet.transform.eulerAngles = new Vector3(0, 135, 0);

        animator.SetFloat("PosZ", xSpeed);
        animator.SetFloat("PosX", -zSpeed);
    }
    void State3()
    {
        feet.transform.eulerAngles = new Vector3(0, 225, 0);

        animator.SetFloat("PosZ", -zSpeed);
        animator.SetFloat("PosX", -xSpeed);
    }
    void State4()
    {
        feet.transform.eulerAngles = new Vector3(0, 315, 0);

        animator.SetFloat("PosZ", -xSpeed);
        animator.SetFloat("PosX", zSpeed);
    }
}
