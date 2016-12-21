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

        //WOMBO COMBO'S!!!

    Animator animator;

    bool _ACool, _CCool;

    float _ACoolT, attackDelay, _CCoolT, comboTime;

    int _Combo = 1;

    public AttackController (Animator _anim, float _attackDelay, float _comboTime)
    {
        animator = _anim;
        attackDelay = _attackDelay;
        comboTime = _comboTime;
    }

    public void InCombat()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && _ACool != true)
        {
            if(_CCool == true)
            {
                _Combo = Mathf.RoundToInt(Random.Range(1, 3));
            }
            animator.SetInteger("Combo", _Combo);
            animator.SetTrigger("Attack");
            _ACool = true;
            _CCool = true;
        }

        if(_ACool == true)
        {
            _ACoolT += Time.deltaTime;

            if(_ACoolT > attackDelay)
            {
                _ACoolT = 0;
                _ACool = false;
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

    }
}
