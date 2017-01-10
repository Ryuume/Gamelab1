using UnityEngine;
using System.Collections;

public class Guard
{
    public float speed, damage, fireRate, health, timer;

    public Transform manager, target;

    NavMeshAgent agent;

    bool _ACool;
    float _ACoolT;

    public Guard(Transform ai, float combatSpeed, float setDamage, float setFireRate, float setHealth)
    {
        manager = ai;
        speed = combatSpeed;
        damage = setDamage;
        fireRate = setFireRate;
        health = setHealth;

        agent = manager.GetComponent<NavMeshAgent>();
    }

    public void Active ()
    {
        float distanceToTarget = Vector3.Distance(target.position, manager.position);

        if (distanceToTarget > 3)
        {
            Move();
        } 
        else
        {
            InCombat();
        }    
    }

    void Move()
    {
        agent.speed = speed;
        agent.SetDestination(target.position);
    }

    void InCombat()
    {
        State1();
    }
    //basic attack state
    void State1()
    {
        agent.speed = 0;
        Vector3 targetPos = target.position;
        targetPos.y = manager.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - manager.position);

        // Smoothly rotate towards the target point.
        manager.rotation = Quaternion.Slerp(manager.rotation, targetRotation, 3 * Time.deltaTime);
        if (_ACool != true)
        {
            Attack();
            _ACool = true;
        }
        else
        {
            _ACoolT += Time.deltaTime;

            if (_ACoolT > fireRate)
            {
                _ACoolT = 0;
                _ACool = false;
                manager.GetComponent<AIManager>().weapon.GetComponent<Katana>().doDamage = false;
            }
        }
    }
    //scared state
    void State2()
    {

    }
    //looking for new enemies
    void State3()
    {

    }

    void Attack()
    {
        Animator animator = manager.GetComponent<AIManager>().animator;
        manager.GetComponent<AIManager>().weapon.GetComponent<Katana>().doDamage = true;
        animator.SetInteger("Combo", Mathf.FloorToInt(Random.Range(1, 4)));
        animator.SetTrigger("Attack");
    }
}
