using UnityEngine;
using System.Collections;

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
    public float combatSpeed, damage, attackDelay, health, wanderRadius;

    [Header("Movement Settings")]
    public Transform path;
    public Transform wanderArea;
    public bool loopPath, wanderInArea;

    [HideInInspector]
    public bool inCombat;

    [Header("Stealth Settings")]
    public float spottingTime;
    public float suspiciousSpottingTime;

    [Header("Ranged Settings")]
    [Tooltip("Add and arrow or other projectile here so the ranged units know what to shoot.")]
    public GameObject projectile;

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
    
    public void Start()
    {
        GetComponent<NavMeshAgent>().speed = speed;
        if(path == null)
        {
            path = transform;
        }

        UnitBehaviour unit = new UnitBehaviour(wanderRadius, speed, transform, path, loopPath);

        switch (mode)
        {
            //Enemy's
            case Mode.enemy:
                {
                    Enemy enemy = new Enemy(transform, combatSpeed, damage, attackDelay, health, unit);
                    eUpdate = enemy;
                    break; 
                }

            case Mode.demon:
                {
                    Demon demon = new Demon(transform, combatSpeed, damage, attackDelay, health, unit);
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
                    Ally ally = new Ally(transform, combatSpeed, damage, attackDelay, health, unit);
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

                            eUpdate.unit.target = null;
                            eUpdate.unit.randomTarget = Vector3.zero;

                            suspicious = true;
                        }
                        if(suspicious == true)
                        {
                            if(seenWhilstSuspicious > (suspiciousSpottingTime * targetDistance / 3) && inCombat == false)
                            {
                                print("I see him!");
                                eUpdate.target = target;
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

    public void FixedUpdate()
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

    public void StartAICoroutine(IEnumerator coroutineMethod)
    {
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
}