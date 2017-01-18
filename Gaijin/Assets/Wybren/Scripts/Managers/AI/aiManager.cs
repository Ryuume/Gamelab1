using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NavMeshAgent))]
public class AIManager : MonoBehaviour
{
    //MUSTS
    //1. allows UX Designer to assign mode of AI. (enemy, npc, ally) &
    //2. allows UX Designer to assign type of AI. (Enemy: Ranged / Melee, NPC: Shop? / Generic / Dialogue?, Ally: Ranged / Melee / Support) &
    //3. allows UX Designer to assign path type of AI. (Stationary, Path, Wander) &
    //4. allows UX Designer to assign attack type (in case of enemy/ally) and which enemy to prioritize. &
    //5. Based on choice, allow UX Designer to assign AI Speed*, Damage*, Health*, Ect. &
    //6. Calls correct class and sends neccesarry information along with them.
    //7. OPTIONAL: When UX Designer has selected NPC: Dialogue, give designer option to insert dialogue file.

    public enum Mode { enemy, demon, npc, ally }

    public enum EnemyType { Guard, Archer, Scout, Assasin, Samurai }
    public enum NpcType { Generic }
    public enum AllyType { Ranged, Melee }

    public enum Priority { HighestDamage, LowestDamage, HighestHealth, LowestHealth }
    public enum PathType { Path, Stationary, Wander }

    public Mode mode = new Mode();
    public EnemyType enemyType = new EnemyType();
    public NpcType npcType = new NpcType();
    public AllyType allyType = new AllyType();
    public Priority priority = new Priority();
    public PathType pathType = new PathType();

    [Header("AI Settings")]
    public float speed;
    public float combatSpeed,  wanderRadius;
    public Animator animator;

    [Header("Movement Settings")]
    public Transform path;
    public Transform wanderArea;
    public bool loopPath, wanderInArea;

    [Header("Combat Settings")]
    public GameObject weapon;
    public GameObject projectile;
    public float minDamage, maxDamage, attackDelay, health, suspiciousHelpRadius, combatHelpRadius;

    [HideInInspector]
    public bool inCombat;

    [Header("Stealth Settings")]
    public float spottingTime;
    public float suspiciousSpottingTime;

    [HideInInspector]
    public Collision collision;
    [HideInInspector]
    public bool seeingSomething, suspicious, visible;
    [HideInInspector]
    public float seenWhilstSuspicious, seenTime, suspiciousCooldown, combatCooldown;
    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public Enemy eUpdate;
    Demon dUpdate;
    Npc npcUpdate;
    Ally aUpdate;

    [HideInInspector]
    public bool freeze, dead;
    IEnumerator savedNumerator;
    
    public void Start()
    {
        GetComponent<NavMeshAgent>().speed = speed;
        if(path == null)
        {
            path = transform;
        }

        weapon.SendMessageUpwards("RecieveData", new Vector2(minDamage, maxDamage));
        UnitBehaviour unit = new UnitBehaviour(wanderRadius, speed, transform, path, loopPath);

        switch (mode)
        {
            //Enemy's
            case Mode.enemy:
                {
                    Enemy enemy = new Enemy(transform, combatSpeed, attackDelay, health, unit);
                    eUpdate = enemy;
                    break; 
                }

            case Mode.demon:
                {
                    Demon demon = new Demon(transform, combatSpeed, attackDelay, health, unit);
                    dUpdate = demon;
                    break;
                }

            //Npc's
            case Mode.npc:
                {
                    Npc npc = new Npc(transform, unit);
                    npcUpdate = npc;
                    break;
                }

            //Ally's
            case Mode.ally:
                {
                    Ally ally = new Ally(transform, combatSpeed, attackDelay, health, unit);
                    aUpdate = ally;
                    break;
                }
      }
    }

    public void CountUp()
    {
        if (suspicious != true)
        {
            seenTime += Time.deltaTime * 1;
        }
        else if (suspicious == true && inCombat != true)
        {
            seenWhilstSuspicious += Time.deltaTime * 1;
        }
    }

    public void Update()
    {
        if (health == 0 && dead != true || health < 0 && dead != true)
        {

            dead = true;
            freeze = true;
            StopCoroutine(savedNumerator);
            animator.SetTrigger("Death");
            print("I cri ervytiem");
            GameObject.Find("GameManager").GetComponent<CombatManager>().EnemyDeath(this);
            //Destroy(gameObject);
        }

        if (freeze != true)
        {
            //print(GetComponent<NavMeshAgent>().velocity + " Velocity");
            if (GetComponent<NavMeshAgent>().velocity.x == 0 && GetComponent<NavMeshAgent>().velocity.z == 0)
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
            }

            
            animator.SetBool("Suspicious", suspicious);
            animator.SetBool("InCombat", inCombat);

            switch (mode)
            {
                //Enemy's
                case Mode.enemy:
                    {
                        if (visible == true)
                        {
                            eUpdate.visible = true;
                            GetComponent<NavMeshAgent>().speed = 0;
                            float targetDistance = Vector3.Distance(target.position, transform.position);

                            if (seenTime > (spottingTime * targetDistance / 6) && suspicious == false)
                            {
                                print("I saw something");

                                NavMeshHit hit;
                                NavMesh.SamplePosition(target.position, out hit, 10, 1);
                                eUpdate.sTarget = hit.position;

                                Collider[] allColliders = Physics.OverlapSphere(transform.position, suspiciousHelpRadius);
                                List<AIManager> _AIList = new List<AIManager>();
                                foreach (Collider _AI in allColliders)
                                {
                                    if (_AI.tag == "Enemy")
                                    {
                                        _AIList.Add(_AI.GetComponent<AIManager>());
                                    }
                                }
                                _AIList.Add(this);
                                GameObject.Find("GameManager").GetComponent<CombatManager>().Suspicious(_AIList, hit.position);

                                eUpdate.unit.target = null;
                                eUpdate.unit.randomTarget = Vector3.zero;

                                suspicious = true;
                            }
                            if (suspicious == true)
                            {
                                if (seenWhilstSuspicious > (suspiciousSpottingTime * targetDistance / 3) && inCombat == false)
                                {
                                    print("I see him!");
                                    eUpdate.target = target;

                                    Collider[] allColliders = Physics.OverlapSphere(transform.position, combatHelpRadius);
                                    List<AIManager> _AIList = new List<AIManager>();
                                    foreach (Collider _AI in allColliders)
                                    {
                                        if (_AI.tag == "Enemy")
                                        {
                                            _AIList.Add(_AI.GetComponent<AIManager>());
                                        }
                                    }
                                    GameObject.Find("GameManager").GetComponent<CombatManager>().SpottedPlayer(_AIList, target);

                                    GetComponent<FieldOfView>().draw = false;
                                    GetComponent<AreaOfView>().draw = false;
                                    inCombat = true;
                                }
                            }
                        }
                        else
                        {
                            eUpdate.visible = false;
                            GetComponent<NavMeshAgent>().speed = speed;

                            if (seenTime > 0)
                            {
                                seenTime -= Time.deltaTime * 1;
                            }
                            if (seenWhilstSuspicious > 0)
                            {
                                seenWhilstSuspicious -= Time.deltaTime * 1;
                            }
                            if (suspicious == true && inCombat == false)
                            {
                                suspiciousCooldown += Time.deltaTime * 1;
                                if (suspiciousCooldown > 12)
                                {
                                    print("I must be imagining things");
                                    suspiciousCooldown = 0;
                                    suspicious = false;
                                }
                            }
                        }
                        break;
                    }
            }
        } 
    }

    public void FixedUpdate()
    {
        if (freeze != true || dead != true)
        {
            switch (mode)
            {
                //Enemy's
                case Mode.enemy:
                    {
                        eUpdate.Active();
                        eUpdate.suspicious = suspicious;
                        eUpdate.inCombat = inCombat;
                        break;
                    }

                case Mode.demon:
                    {
                        dUpdate.Active();
                        dUpdate.suspicious = suspicious;
                        dUpdate.inCombat = inCombat;
                        break;
                    }

                //Npc's
                case Mode.npc:
                    {
                        npcUpdate.Active();
                        npcUpdate.inCombat = inCombat;
                        break;
                    }

                //Ally's
                case Mode.ally:
                    {
                        aUpdate.Active();
                        aUpdate.inCombat = inCombat;
                        break;
                    }
            }
        }
    }

    public void StartAICoroutine(IEnumerator coroutineMethod)
    {
        savedNumerator = coroutineMethod;
        StartCoroutine(coroutineMethod);
    }

    public void OnCollisionEnter(Collision col)
    {
        collision = col;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        if (dead != true || freeze != true)
        {
            print("hit");
            health -= damage;
            if(health > 0)
            {
                animator.SetTrigger("Hit");
            }
        }
    }

    public void Stun()
    {
        if (dead != true)
        {
            print("Stun");
            animator.SetBool("Stunned", true);
        }
    }

    public void Smoked()
    {
        if (dead != true)
        {
            animator.SetTrigger("Smoke");
            freeze = true;
        }
    }
}