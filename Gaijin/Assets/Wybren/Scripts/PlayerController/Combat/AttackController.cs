using UnityEngine;
using System.Collections;

public class AttackController
{
    //TODO
    //Different classes for each weapon
    //Katana, Kusurigama, Shurikens, Smokebombs
    //Kusurigama, Shurikens and Smokebombs are abilities that have a cooldown on them. Katana is constantly useable.
    //When hit, sent damage and if stunned.
    //Weapons and abilities use their own trigger or collider to register when they hit an enemy. Same goes for the weapons of the enemies.

    //WOMBO COMBO!!!

    Animator animator;

    bool _ACool, _CCool, rotating;

    public float _ACoolT, attackDelay, _CCoolT, comboTime, rotationLeft = 0, originalRot, standardRot;

    public int _Combo = 1, stateNum;

    GameObject katana, player, model;
    Transform lHand;

    public AttackController (Animator _anim, float _attackDelay, float _comboTime, GameObject _Katana, GameObject _player, int _StateNum, float _standardRot, GameObject _playerModel, Transform _LHand)
    {
        animator = _anim;
        attackDelay = _attackDelay;
        comboTime = _comboTime;
        katana = _Katana;
        player = _player;
        stateNum = _StateNum;
        standardRot = _standardRot;
        model = _playerModel;
        lHand = _LHand;
    }

    public void InCombat()
    {
        if (stateNum == 1)
            originalRot = standardRot;
        else if (stateNum == 2)
            originalRot = standardRot + 90;
        else if (stateNum == 3)
            originalRot = standardRot + 180;
        else if (stateNum == 4)
            originalRot = standardRot + 270;

        Attack();
        Abillity();
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && _ACool != true && rotating != true)
        {
            if(_CCool == true)
            {
                _Combo = Mathf.CeilToInt(Random.Range(1, 4));
                if(_Combo == 3)
                {
                    if(Mathf.CeilToInt(Random.Range(1, 4)) != 3)
                    {
                        _Combo = Mathf.CeilToInt(Random.Range(1, 3));
                    }
                }
            }
            katana.GetComponent<Katana>().doDamage = true;
            animator.SetInteger("Combo", _Combo);
            animator.SetTrigger("Attack");
            _ACool = true;
            _CCool = true;
        }

        if (_Combo == 3)
        {
            rotating = true;
        }

        if (rotating == true)
        {
            Rotate();
            katana.GetComponent<Katana>().doDamage = true;
        }
        else
        {
            rotationLeft = 0;
        }

        if(_ACool == true)
        {
            _ACoolT += Time.deltaTime;

            if(_ACoolT > attackDelay)
            {
                _ACoolT = 0;
                _ACool = false;
                katana.GetComponent<Katana>().doDamage = false;
            }
        }

        if(_CCool == true)
        {
            _CCoolT += Time.deltaTime;

            if (_CCoolT > comboTime)
            {
                _CCoolT = 0;
                _Combo = 1;
                _CCool = false;
            }
        }
    }

    void Abillity()
    {
        if(Input.GetButtonDown("Fire3"))
        {
            player.GetComponent<PlayerController>().katana.transform.parent = lHand;
            animator.SetTrigger("Shuriken");
        }
    }

    void Rotate()
    {
        if(rotationLeft < 360)
        {
            Debug.Log(1);
            rotationLeft += 25;
            model.transform.rotation = Quaternion.Euler(0, originalRot + rotationLeft, 0);
        }else
        {
            model.transform.rotation = Quaternion.Euler(0, originalRot, 0);
            katana.GetComponent<Katana>().doDamage = false;
            rotating = false;
            Debug.Log("reset");
        }
    }
}
