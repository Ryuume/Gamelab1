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
        GetComponent<NavMeshAgent>().speed = speed;
        UnitBehaviour unit = new UnitBehaviour(wanderRadius, speed, transform, path, loopPath);
        switch (mode)
        {
            //Enemy's
            case Mode.enemy:
                {
                    Enemy enemy = new Enemy(transform, damage, health, unit);
                    eUpdate = enemy;
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
                    Ally ally = new Ally(transform, damage, health, unit);
                    aUpdate = ally;
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
                                npcUpdate.Generic();
                                npcUpdate.inCombat = inCombat;
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