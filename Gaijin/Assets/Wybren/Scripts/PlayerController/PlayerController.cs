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

    public float standardRotation;

    Vector3 mousePos, top, bottom, left, right;
    enum lookState { top, bottom, left, right }
    lookState currentState;

    public float speed, length;
    public float xSpeed, zSpeed;
    public Animator animator;

    public float minDamage, maxDamage;
    public GameObject katana;

    bool setRotation = false, moving, turnRight, setFloat, isTurning, wielding, inCombat;

    float moveFloat;

    AttackController attackController;
    int stateNum; //1 = top, 2 = right, 3 = bottom, 4 = left.

    public void Start()
    {
        currentState = lookState.top;

        top = refDir.forward;
        bottom = -refDir.forward;
        left = -refDir.right;
        right = refDir.right;

        katana.GetComponent<Katana>().minDamage = minDamage;
        katana.GetComponent<Katana>().maxDamage = maxDamage;
        attackController = new AttackController(animator, 0.4f, 1, katana, transform.gameObject, stateNum, standardRotation, feet.gameObject);
    }

    public void Update()
    {
        lookStates();
        Move();
        AnimationInput();
        attackController.stateNum = stateNum;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(transform.position, top, Color.red);
        Debug.DrawRay(transform.position, bottom, Color.blue);
        Debug.DrawRay(transform.position, left, Color.green);
        Debug.DrawRay(transform.position, right, Color.yellow);

        Debug.DrawRay(transform.position, new Vector3(1, 0, 0), Color.white);
        Debug.DrawRay(transform.position, new Vector3(-1, 0, 0), Color.white);
        Debug.DrawRay(transform.position, new Vector3(0, 0, 1), Color.white);
        Debug.DrawRay(transform.position, new Vector3(0, 0, -1), Color.white);

        if(Input.GetButtonDown("Ready"))
        {
            wielding = !wielding;
            animator.SetTrigger("Draw");
            animator.SetBool("Wielding", wielding);
            animator.SetBool("InCombat", false);
        }
        if (wielding == true && Input.GetButtonDown("TempCombat"))
        {
            inCombat = !inCombat;
            animator.SetBool("InCombat", inCombat);
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
        {
            Vector3 targetPos = hit.point;
            Debug.DrawLine(transform.position, targetPos, Color.blue);
            if (Vector3.Distance(transform.position, targetPos) > .5f)
            { 
                targetPos.y = head.position.y;
                
                targetPos.y = feet.position.y;
                mousePos = targetPos;
            }
        }
    }

    public void LateUpdate()
    {
        if (inCombat == true)
            attackController.InCombat();
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

        if(xSpeed > 0 || zSpeed > 0 || xSpeed < 0 || zSpeed < 0)
        {
            moving = true;
            animator.SetBool("Moving", true);
        }else
        {
            moving = false;
            animator.SetBool("Moving", false);
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
                    SwitchState1();
                    State1();
                    stateNum = 1;
                    break;
                }
            case lookState.bottom:
                {
                    SwitchState3();
                    State3();
                    stateNum = 3;
                    break;
                }
            case lookState.left:
                {
                    SwitchState4();
                    State4();
                    stateNum = 4;
                    break;
                }
            case lookState.right:
                {
                    SwitchState2();
                    State2();
                    stateNum = 2;
                    break;
                }
        }

        
    }

    void SwitchState1()
    {
        var mousePosition = Input.mousePosition;
        var playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(top, targetDir);
        if(mousePosition.x  > playerPosition.x)
        {
           // print("1");
        }else
        {
            angle = (-angle);
        }

        animator.SetFloat("Rotation", angle);

        if (angle > 50)
        {
            setRotation = false;
            turnRight = true;
            setFloat = false;
            currentState = lookState.right;
        }else if (angle < -50)
        {
            setRotation = false;
            turnRight = false;
            setFloat = false;
            currentState = lookState.left;
        }
    }
    void SwitchState2()
    {
        var mousePosition = Input.mousePosition;
        var playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(right, targetDir);
        if (mousePosition.y < playerPosition.y)
        {
            //print("2");
        }else
        {
            angle = (-angle);
        }

        animator.SetFloat("Rotation", angle);

        if (angle > 50)
        {
            setRotation = false;
            turnRight = true;
            setFloat = false;
            currentState = lookState.bottom;
        }else if (angle < -50)
        {
            setRotation = false;
            turnRight = false;
            setFloat = false;
            currentState = lookState.top;
        }
    }
    void SwitchState3()
    {
        var mousePosition = Input.mousePosition;
        var playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(bottom, targetDir);
        if (mousePosition.x < playerPosition.x)
        {
           // print("3");
        }else
        {
            angle = (-angle);
        }

        animator.SetFloat("Rotation", angle);

        if (angle > 50)
        {
            setRotation = false;
            turnRight = true;
            setFloat = false;
            currentState = lookState.left;
        }else if (angle < -50)
        {
            setRotation = false;
            turnRight = false;
            setFloat = false;
            currentState = lookState.right;
        }
    }
    void SwitchState4()
    {
        var mousePosition = Input.mousePosition;
        var playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 targetDir = (mousePos - transform.position);
        float angle = Vector3.Angle(left, targetDir);
        if (mousePosition.y > playerPosition.y)
        {
           //print("4");
        }else
        {
            angle = (-angle);
        }

        animator.SetFloat("Rotation", angle);

        if (angle > 50)
        {
            setRotation = false;
            turnRight = true;
            setFloat = false;
            currentState = lookState.top;
        }else if (angle < -50)
        {
            setRotation = false;
            turnRight = false;
            setFloat = false;
            currentState = lookState.bottom;
        }
    }

    void State1()
    {
        if (setRotation != true)
        {
            Turn(standardRotation);
        }
        else
        {
            moveFloat = 1;
            Vector3 targetDir = (mousePos - transform.position);
            float angle = Vector3.Angle(top, targetDir);

            if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(transform.position).x)
            {
                angle = (-angle);
            }

            animator.SetFloat("Look", angle);
        }
    }
    void State2()
    {
        if (setRotation != true)
        {
            Turn(standardRotation + 90);
        }
        else
        {
            moveFloat = 2;
            Vector3 targetDir = (mousePos - transform.position);
            float angle = Vector3.Angle(right, targetDir);

            if (Input.mousePosition.y > Camera.main.WorldToScreenPoint(transform.position).y)
            {
                angle = (-angle);
            }

            animator.SetFloat("Look", angle);
        }
    }
    void State3()
    {
        if (setRotation != true)
        {
            Turn(standardRotation + 180);
        }
        else
        {
            moveFloat = 3;
            Vector3 targetDir = (mousePos - transform.position);
            float angle = Vector3.Angle(bottom, targetDir);

            if (Input.mousePosition.x > Camera.main.WorldToScreenPoint(transform.position).x)
            {
                angle = (-angle);
            }

            animator.SetFloat("Look", angle);
        }
    }
    void State4()
    {
        if (setRotation != true)
        {
            Turn(standardRotation + 270);
        }
        else
        {
            moveFloat = 4;
            Vector3 targetDir = (mousePos - transform.position);
            float angle = Vector3.Angle(left, targetDir);

            if (Input.mousePosition.y < Camera.main.WorldToScreenPoint(transform.position).y)
            {
                angle = (-angle);
            }

            animator.SetFloat("Look", angle);
        }
    }

    public void Turn(float DesiredRotation)
    {
        if(moving != true || isTurning == true || inCombat == true)
        {
            isTurning = true;
            float newAngle = Mathf.LerpAngle(feet.eulerAngles.y, DesiredRotation, Time.deltaTime * 4);
            feet.eulerAngles = new Vector3(0, newAngle, 0);
        }

        switch (currentState)
        {
            case lookState.top:
                {
                    if (Mathf.RoundToInt(feet.localEulerAngles.y) == 0 || Mathf.RoundToInt(feet.localEulerAngles.y) == 360)
                    {
                        print("Bla");
                        isTurning = false;
                        setRotation = true;
                        moveFloat = 1;
                    }
                    else if(Input.GetButton("Down") || (Input.GetButton("Up") && (moveFloat == 3 || moveFloat == 1)))
                    {
                        moveFloat = 1;
                        float newAngle = Mathf.LerpAngle(feet.eulerAngles.y, DesiredRotation, Time.deltaTime * 4);
                        feet.eulerAngles = new Vector3(0, newAngle, 0);
                    }

                    if(isTurning == true)
                    {
                        moveFloat = 1;
                    }
                    break;
                }
            case lookState.bottom:
                {
                    if (Mathf.RoundToInt(feet.localEulerAngles.y) == 180)
                    {
                        isTurning = false;
                        setRotation = true;
                        moveFloat = 3;
                    }
                    else if (Input.GetButton("Up") || (Input.GetButton("Down") && (moveFloat == 1 || moveFloat == 3)))
                    {
                        moveFloat = 3;
                        float newAngle = Mathf.LerpAngle(feet.eulerAngles.y, DesiredRotation, Time.deltaTime * 4);
                        feet.eulerAngles = new Vector3(0, newAngle, 0);
                    }
                    if (isTurning == true)
                    {
                        moveFloat = 3;
                    }
                    break;
                }
            case lookState.left:
                {
                    if (Mathf.RoundToInt(feet.localEulerAngles.y) == 270)
                    {
                        isTurning = false;
                        setRotation = true;
                        moveFloat = 4;
                    }
                    else if (Input.GetButton("Right") || (Input.GetButton("Left") && (moveFloat == 2 || moveFloat == 4)))
                    {
                        moveFloat = 4;
                        float newAngle = Mathf.LerpAngle(feet.eulerAngles.y, DesiredRotation, Time.deltaTime * 4);
                        feet.eulerAngles = new Vector3(0, newAngle, 0);
                    }
                    if (isTurning == true)
                    {
                        moveFloat = 4;
                    }
                    break;
                }
            case lookState.right:
                {
                    if (Mathf.RoundToInt(feet.localEulerAngles.y) == 90)
                    {
                        isTurning = false;
                        setRotation = true;
                        moveFloat = 2;
                    }
                    else if (Input.GetButton("Left") || (Input.GetButton("Right") && (moveFloat == 4 || moveFloat == 2)))
                    {
                        moveFloat = 2;
                        float newAngle = Mathf.LerpAngle(feet.eulerAngles.y, DesiredRotation, Time.deltaTime * 4);
                        feet.eulerAngles = new Vector3(0, newAngle, 0);
                    }
                    if (isTurning == true)
                    {
                        moveFloat = 2;
                    }
                    break;
                }
        }
    }

    void AnimationInput()
    {
        if(moveFloat == 1)
        {
            animator.SetFloat("PosZ", zSpeed);
            animator.SetFloat("PosX", xSpeed);
        }else if (moveFloat == 2)
        {
            animator.SetFloat("PosZ", xSpeed);
            animator.SetFloat("PosX", -zSpeed);
        }else if(moveFloat == 3)
        {
            animator.SetFloat("PosZ", -zSpeed);
            animator.SetFloat("PosX", -xSpeed);
        }else if (moveFloat == 4)
        {
            animator.SetFloat("PosZ", -xSpeed);
            animator.SetFloat("PosX", zSpeed);
        }
    }
}
