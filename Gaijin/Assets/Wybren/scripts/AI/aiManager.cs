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

    public enum Mode { enemy, npc, ally }

    public enum EnemyType { Ranged, Melee }
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

    public float speed, damage, health, wanderRadius;

    public Transform path, wanderArea;

    public bool loopPath, wanderInArea, inCombat;

    public Enemy eUpdate;
    public Npc npcUpdate;
    public Ally aUpdate;
    
    public void Start()
    {
        switch (mode)
        {
            //Enemy's
            case Mode.enemy:
                {
                    UnitBehaviour enemy = new UnitBehaviour();
                    enemy.manager = transform;
                    enemy.loop = loopPath;

                    foreach (Transform child in path)
                    {
                        enemy.waypoints.Add(child);
                    }

                    switch (enemyType)
                    {
                        case EnemyType.Ranged:
                            {
                                /*
                                Enemy eRanged = new Enemy();
                                eRanged.manager = transform;
                                eRanged.path = path;
                                eRanged.health = health;
                                eRanged.damage = damage;
                                eRanged.speed = speed;
                                eRanged.loop = loopPath;
                                eRanged.GetStates();
                                eUpdate = eRanged;
                                */
                                break;
                                
                            }
                        case EnemyType.Melee:
                            {
                                /*
                                Enemy eMelee = new Enemy();
                                eMelee.manager = transform;
                                eMelee.path = path;
                                eMelee.health = health;
                                eMelee.damage = damage;
                                eMelee.speed = speed;
                                eMelee.loop = loopPath;
                                eMelee.GetStates();
                                eUpdate = eMelee;
                                */
                                break;
                                
                            }
                    }
                    break;
                }

            //Npc's
            case Mode.npc:
                {
                    UnitBehaviour npc = new UnitBehaviour();

                    switch (npcType)
                    {
                        case NpcType.Generic:
                            {
                                /*
                                Npc generic = new Npc();
                                generic.manager = transform;
                                generic.path = path;
                                generic.health = health;
                                generic.speed = speed;
                                generic.loop = loopPath;
                                generic.walkRadius = wanderRadius;
                                generic.GetStates();
                                npcUpdate = generic;
                                */
                                break;
                            }
                    }
                    break;
                }

            //Ally's
            case Mode.ally:
                {
                    UnitBehaviour ally = new UnitBehaviour();

                    switch (allyType)
                    {
                        case AllyType.Ranged:
                            {
                                /*
                                Ally aRanged = new Ally();
                                aRanged.manager = transform;
                                aRanged.path = path;
                                aRanged.health = health;
                                aRanged.damage = damage;
                                aRanged.speed = speed;
                                aRanged.GetStates();
                                aUpdate = aRanged;
                                */
                                break;
                            }
                        case AllyType.Melee:
                            {
                                /*
                                Ally aMelee = new Ally();
                                aMelee.manager = transform;
                                aMelee.path = path;
                                aMelee.health = health;
                                aMelee.damage = damage;
                                aMelee.speed = speed;
                                aMelee.GetStates();
                                aUpdate = aMelee;
                                */
                                break;
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
                    switch (enemyType)
                    {
                        case EnemyType.Ranged:
                            {
                                eUpdate.Ranged();
                                break;
                            }
                        case EnemyType.Melee:
                            {
                                eUpdate.Melee();
                                break;
                            }
                    }
                    break;
                }

            //Npc's
            case Mode.npc:
                {
                    switch (npcType)
                    {
                        case NpcType.Generic:
                            {
                                npcUpdate.inCombat = inCombat;
                                npcUpdate.Generic();
                                break;
                            }
                    }
                    break;
                }

            //Ally's
            case Mode.ally:
                {
                    switch (allyType)
                    {
                        case AllyType.Ranged:
                            {
                                aUpdate.Ranged();
                                break;
                            }
                        case AllyType.Melee:
                            {
                                aUpdate.Melee();
                                break;
                            }
                    }

                    break;
                }

        }
    }

    public void StartAICoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }
    
}