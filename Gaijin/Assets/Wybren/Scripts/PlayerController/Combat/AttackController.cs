using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    bool _ACool, _CCool, rotating, _SCool, _KCool, _BCool, _DCool;

    public float _ACoolT, attackDelay, _CCoolT, comboTime, rotationLeft = 0, originalRot, standardRot, shurikenCooldown, kusarigamaCooldown, smokebombCooldown, _SCoolT, _KCoolT, _BCoolT, _DCoolT;

    public int _Combo = 1, stateNum, rotSpeed = 700;

    GameObject katana, player, model, kusarigama;

    Image katanaImg, kusarigamaImg, shurikenImg, smokeImg;

    public AttackController (Animator _anim, float _attackDelay, float _comboTime, GameObject _Katana, GameObject _player, int _StateNum, float _standardRot, GameObject _playerModel, GameObject _Kusarigama, float _ShurikenCooldown, float _KusarigamaCooldown, float _SmokebombCoolDown, Image _katanaImg, Image _kusarigamaImg, Image _shurikenImg, Image _SmokeImg)
    {
        animator = _anim;
        attackDelay = _attackDelay;
        comboTime = _comboTime;
        katana = _Katana;
        player = _player;
        stateNum = _StateNum;
        standardRot = _standardRot;
        model = _playerModel;
        kusarigama = _Kusarigama;
        shurikenCooldown = _ShurikenCooldown;
        kusarigamaCooldown = _KusarigamaCooldown;
        smokebombCooldown = _SmokebombCoolDown;
        katanaImg = _katanaImg;
        kusarigamaImg = _kusarigamaImg;
        shurikenImg = _shurikenImg;
        smokeImg = _SmokeImg;
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
                    }else
                    {
                        rotating = true;
                    }
                }
            }
            katana.GetComponent<Katana>().doDamage = true;
            animator.SetInteger("Combo", _Combo);
            animator.SetTrigger("Attack");
            _ACool = true;
            _CCool = true;
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
            katanaImg.fillAmount = _ACoolT / attackDelay;

            if (_ACoolT > attackDelay)
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
        if(Input.GetButtonDown("Fire3") && rotating == false && _ACool == false && _SCool == false)
        {
            katana.SetActive(false);
            animator.SetTrigger("Shuriken");
            _SCool = true;
        }

        if(Input.GetButtonDown("Kusarigama") && rotating == false && _ACool == false && _KCool == false)
        {
            katana.SetActive(false);
            kusarigama.SetActive(true);
            animator.SetTrigger("Kusarigama");
            rotSpeed = 800;
            rotating = true;
            _KCool = true;
        }

        if(Input.GetButtonDown("SmokeBomb") && rotating == false && _ACool == false && _BCool == false)
        {
            player.GetComponent<PlayerController>().katana.SetActive(false);
            animator.SetTrigger("Smokebomb");
            _BCool = true;
        }

        if (Input.GetButtonDown("DragonPunch") && rotating == false && _ACool == false && player.GetComponent<PlayerController>().dragonPunchActive == true && player.GetComponent<PlayerController>().dragonPunchCharge == 100) 
        {
            player.GetComponent<PlayerController>().dragonPunchCharge = 0;
            animator.SetTrigger("DragonPunch");
        }

        #region cooldowns
        if (_SCool == true)
        {
            _SCoolT += Time.deltaTime;
            shurikenImg.fillAmount = _SCoolT / shurikenCooldown;

            if (_SCoolT > shurikenCooldown)
            {
                _SCoolT = 0;
                _SCool = false;
            }
        }
        if (_KCool == true)
        {
            _KCoolT += Time.deltaTime;
            kusarigamaImg.fillAmount = _KCoolT / kusarigamaCooldown;

            if (_KCoolT > kusarigamaCooldown)
            {
                _KCoolT = 0;
                _KCool = false;
            }
        }
        if (_BCool == true)
        {
            _BCoolT += Time.deltaTime;
            smokeImg.fillAmount = _BCoolT / smokebombCooldown;

            if (_BCoolT > smokebombCooldown)
            {
                _BCoolT = 0;
                _BCool = false;
            }
        }
        #endregion
    }

    void Rotate()
    {
        if(rotationLeft < 360)
        {
            rotationLeft += rotSpeed * Time.deltaTime;
            model.transform.rotation = Quaternion.Euler(0, originalRot + rotationLeft, 0);
        }else
        {
            model.transform.rotation = Quaternion.Euler(0, originalRot, 0);
            katana.GetComponent<Katana>().doDamage = false;
            rotSpeed = 700;
            rotating = false;
        }
    }
}
