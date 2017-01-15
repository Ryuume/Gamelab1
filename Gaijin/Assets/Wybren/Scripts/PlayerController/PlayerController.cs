using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //TODO
    //7. Register hits, death and such and play animations based on this.

    [Header("Player Attributes")]
    #region Neccesary Variables
    public LayerMask targetMask;
    public Transform refDir, feet, head, rHand, pelvis;
    public float standardRotation;
    public Animator animator;
    [HideInInspector]
    public int stateNum; //1 = top, 2 = right, 3 = bottom, 4 = left.
    #endregion

    [Header("Movement Settings")]
    #region Movement Variables
    public float speed;
    public float length;
    [HideInInspector]
    public float xSpeed, zSpeed;
    [HideInInspector]
    public bool freeze;
    #endregion

    [Header("Combat Settings")]
    #region Combat Variables
    public float health;
    public float healthRegenTime, minDamage, maxDamage;
    public GameObject katana, shuriken, kusarigama, smokebomb;
    public float shurikenCooldown, kusarigamaCooldown, smokebombCooldown, dragonPunchRegen, hitCooldown;
    //[HideInInspector]
    public bool dragonPunchActive;
    #endregion

    [Header("UI Settings")]
    #region UI Variables
    public Image healthBar;
    public Image dragonBarL, dragonBarR;
    public GameObject dragonPopup;
    public Image katanaImg, kusarigamaImg, shurikenImg, smokeImg;
    #endregion

    #region Script Variables
    RaycastHit hit;
    Vector3 mousePos, top, bottom, left, right;
    enum lookState { top, bottom, left, right }
    lookState currentState;
    bool setRotation = false, moving, isTurning, wielding, inCombat, wasHit, regenDragonPunch;
    float moveFloat, hitTimer, regenHealth = 100;
    public float dragonPunchCharge = 100;
    AttackController attackController;
    #endregion

    public void Start()
    {
        currentState = lookState.top;

        top = refDir.forward;
        bottom = -refDir.forward;
        left = -refDir.right;
        right = refDir.right;

        katana.GetComponent<Katana>().minDamage = minDamage;
        katana.GetComponent<Katana>().maxDamage = maxDamage;
        attackController = new AttackController(animator, 0.4f, 1, katana, transform.gameObject, stateNum, standardRotation, feet.gameObject, kusarigama, shurikenCooldown, kusarigamaCooldown, smokebombCooldown, katanaImg, kusarigamaImg, shurikenImg, smokeImg);
    }

    public void Update()
    {
        if (health == 0 || health < 0)
        {
            print("You Died");
            Destroy(gameObject);
        }

        if (dragonPunchActive == true)
        {
            dragonBarL.fillAmount = Mathf.Lerp(dragonBarL.fillAmount, (dragonPunchCharge / 100), 2 * Time.deltaTime);
            dragonBarR.fillAmount = Mathf.Lerp(dragonBarL.fillAmount, (dragonPunchCharge / 100), 2 * Time.deltaTime);

            if (dragonBarL.fillAmount < 0.1 || regenDragonPunch == true)
            {
                dragonPopup.SetActive(false);
                regenDragonPunch = true;
                dragonPunchCharge += dragonPunchRegen * Time.deltaTime;
            }
            if (dragonBarL.fillAmount > 0.99 || dragonBarL.fillAmount == 1)
            {
                dragonPopup.gameObject.SetActive(true);
                regenDragonPunch = false;
                dragonPunchCharge = 100;
            }
        }

        if(freeze != true)
        {
            SecondaryUpdate();
        }
    }

    void SecondaryUpdate()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / 100), 2 * Time.deltaTime);

        if (wasHit == true)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer > hitCooldown)
            {
                print("lewl");
                wasHit = false;
            }
        }
        else
        {
            if (health < 25 || health == 25)
            {
                regenHealth = 25;
            }
            else if (health < 50 || health == 50)
            {
                regenHealth = 50;
            }
            else if (health < 75 || health == 75)
            {
                regenHealth = 75;
            }
            else if (health < 100 || health == 100)
            {
                regenHealth = 100;
            }
            health = Mathf.Lerp(health, regenHealth, healthRegenTime * Time.deltaTime);
        }

        lookStates();
        Move();
        AnimationInput();
        attackController.stateNum = stateNum;
        attackController.InCombat();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Ready"))
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
            currentState = lookState.right;
        }else if (angle < -50)
        {
            setRotation = false;
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
            currentState = lookState.bottom;
        }else if (angle < -50)
        {
            setRotation = false;
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
            currentState = lookState.left;
        }else if (angle < -50)
        {
            setRotation = false;
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
            currentState = lookState.top;
        }else if (angle < -50)
        {
            setRotation = false;
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

    public void Shuriken()
    {
        Instantiate(shuriken, rHand.position, Quaternion.identity);
    }

    public void SmokeBomb()
    {
        Instantiate(smokebomb, rHand.position, Quaternion.identity);
    }

    public void Hit(float damage)
    {
        health -= damage;
        wasHit = true;
        hitTimer = 0f;
        animator.SetTrigger("Hit");
    }

    public void Stun()
    {
        print("Stun");
        animator.SetBool("Stunned", true);
    }
}
