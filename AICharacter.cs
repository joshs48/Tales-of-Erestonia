using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(BaseCharacter))]
public class AICharacter : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public BaseCharacter character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for
    private Animator anim;

    private string enemy;

    private string weaponType;

    public bool canAtk = true;
    private int atkCount = 0;
    private int basicAtks = 0;
    private int cooldown = 0;
    private float stopDist = 0;
    private float speed = 0;




    private bool basicAtk = false;
    private bool shield = false;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<BaseCharacter>();

        agent.updateRotation = true;
        agent.updatePosition = true;
        playerRef = FindNearestPlayer();
        StartCoroutine(FOVRoutine());
        //SetTarget();

        enemy = name.Substring(0, name.IndexOf(" "));
        weaponType = name.Substring(name.IndexOf(" ") + 1);

        anim = GetComponent<Animator>();
        switch (enemy)
        {
            case "Dwarf":
                anim.SetInteger("Enemy", 0);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "Mystic":
                anim.SetInteger("Enemy", 1);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 3;
                break;
            case "BigOrk":
                anim.SetInteger("Enemy", 2);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 6;
                break;
            case "PigButcher":
                anim.SetInteger("Enemy", 3);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 5;
                break;
            case "AncientQueen":
                anim.SetInteger("Enemy", 4);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 3;
                break;
            case "AncientWarrior":
                anim.SetInteger("Enemy", 5);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 7;
                break;
            case "BarbarianGiant":
                anim.SetInteger("Enemy", 6);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "EarthGolem":
                anim.SetInteger("Enemy", 7);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "FortGolem":
                anim.SetInteger("Enemy", 8);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 10;
                break;
            case "MechGolem":
                anim.SetInteger("Enemy", 9);
                GetComponent<StatsManager>().maxStamina = 5;
                GetComponent<StatsManager>().stamina = 5;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 7;
                break;
            case "MutantGuy":
                anim.SetInteger("Enemy", 10);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 5;
                break;
            case "RedDemon":
                anim.SetInteger("Enemy", 11);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "Slayer":
                anim.SetInteger("Enemy", 12);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 6;
                break;
            case "Troll":
                anim.SetInteger("Enemy", 13);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 7;
                break;
            case "DarkElf":
                anim.SetInteger("Enemy", 14);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 7;
                break;
            case "EvilGod":
                anim.SetInteger("Enemy", 15);
                GetComponent<StatsManager>().maxStamina = 7;
                GetComponent<StatsManager>().stamina = 7;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 9;
                break;
            case "ForestGuardian":
                anim.SetInteger("Enemy", 16);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "ForestWitch":
                anim.SetInteger("Enemy", 17);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "Medusa":
                anim.SetInteger("Enemy", 18);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "SpiritDemon":
                anim.SetInteger("Enemy", 19);
                GetComponent<StatsManager>().maxStamina = 6;
                GetComponent<StatsManager>().stamina = 6;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "GoblinBasic":
                anim.SetInteger("Enemy", 20);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "GoblinWarChief":
                anim.SetInteger("Enemy", 21);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "Ghost":
                anim.SetInteger("Enemy", 22);
                GetComponent<StatsManager>().maxStamina = 5;
                GetComponent<StatsManager>().stamina = 5;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 5;
                break;
            case "TormentedSoul":
                anim.SetInteger("Enemy", 23);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 7;
                break;
            case "SkeletonBasic":
                anim.SetInteger("Enemy", 24);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "SkeletonSoldier":
                anim.SetInteger("Enemy", 25);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 5;
                break;
            case "SkeletonSlave":
                anim.SetInteger("Enemy", 26);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "SkeletonKnight":
                anim.SetInteger("Enemy", 27);
                GetComponent<StatsManager>().maxStamina = 4;
                GetComponent<StatsManager>().stamina = 4;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 6;
                break;
            case "GoblinWarrior":
                anim.SetInteger("Enemy", 28);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 5;
                break;
            case "RockGolem":
                anim.SetInteger("Enemy", 28);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 6;
                break;
            case "IceGolem":
                anim.SetInteger("Enemy", 29);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 8;
                break;
            case "CrystalGolem":
                anim.SetInteger("Enemy", 30);
                GetComponent<StatsManager>().maxStamina = 1;
                GetComponent<StatsManager>().stamina = 1;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 9;
                break;
            case "GoblinShaman":
                anim.SetInteger("Enemy", 31);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "FemalePirate":
                anim.SetInteger("Enemy", 32);
                GetComponent<StatsManager>().maxStamina = 3;
                GetComponent<StatsManager>().stamina = 3;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
            case "DwarfSoldier":
                anim.SetInteger("Enemy", 33);
                GetComponent<StatsManager>().maxStamina = 2;
                GetComponent<StatsManager>().stamina = 2;
                GetComponent<StatsManager>().staminaRechargeSpeed = 1;
                cooldown = 4;
                break;
        }
        stopDist = agent.stoppingDistance;
        speed = agent.speed;

        switch (weaponType)
        {
            case "Single Sword A":
                anim.SetInteger("Weapon Type", 1);
                basicAtks = 4;
                break;
            case "Single Sword B":
                anim.SetInteger("Weapon Type", 2);
                basicAtks = 4;
                break;
            case "Two Swords":
                anim.SetInteger("Weapon Type", 3);
                basicAtks = 3;
                break;
            case "Giant Hammer":
                anim.SetInteger("Weapon Type", 4);
                basicAtks = 3;
                break;
            case "Staff":
                anim.SetInteger("Weapon Type", 5);
                basicAtks = 2;
                break;
            case "Axe":
                anim.SetInteger("Weapon Type", 6);
                basicAtks = 3;
                break;
            case "Martial Brute":
                anim.SetInteger("Weapon Type", 7);
                basicAtks = 3;
                break;
            case "Magic":
                anim.SetInteger("Weapon Type", 8);
                basicAtks = 2;
                break;
            case "Sword and Shield":
                anim.SetInteger("Weapon Type", 9);
                basicAtks = 3;
                break;

        }

    }


    private void Update()
    {
        if (target != null && canAtk)
        {
            agent.SetDestination(target.position);

        }
        float difAngle = 0;
        Vector3 targetDir;
        if (target != null)
        {
            targetDir = target.position - transform.position;
            difAngle = Math.Abs(Vector3.SignedAngle(targetDir, transform.forward, Vector3.forward));
        }

        if (target != null && (agent.remainingDistance > agent.stoppingDistance || difAngle > 20f))
        {

            if (difAngle > 10)
            {
                agent.stoppingDistance = 0.1f;
                //agent.speed = speed / 2;
            }
            else
            {
                agent.stoppingDistance = stopDist;
                //agent.speed = speed;
            }
            character.Move(agent.desiredVelocity, false, false, false, false, shield);
        }
        else if (target != null)
        {
            character.Move(Vector3.zero, false, false, basicAtk, false, shield);
            basicAtk = false;
            if (canAtk)
            {
                if (GetComponent<StatsManager>().stamina > 0 && !GetComponent<StatsManager>().isRechargingStamina)
                {
                    if (enemy.Equals("MechGolem") && weaponType.Equals("Axe"))
                    {
                        StartCoroutine(RotateSaw());
                    }
                    Attack();
                }

            }
        }
        else
        {
            character.Move(Vector3.zero, false, false, false, false, shield);

        }

    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }



    public void Attack()
    {

        basicAtk = true;
        anim.SetInteger("Random Atk Val", UnityEngine.Random.Range(1, basicAtks + 1));

        StartCoroutine(RechargeAtk(anim.GetCurrentAnimatorStateInfo(0).length));
    }



    public IEnumerator RechargeAtk(float time)
    {
        canAtk = false;
        yield return new WaitForSeconds(time);
        canAtk = true;
    }


    public IEnumerator CoolAtk()
    {
        Vector3 targetPos = transform.localPosition - Vector3.back * 2;


        //agent.SetDestination(targetPos);
        //agent.stoppingDistance = 1;
        anim.SetInteger("Random Atk Val", 0);
        canAtk = false;

        /*while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return new WaitForSeconds(0.1f);
        }*/
        /*FindNearestPlayer();
        agent.SetDestination(target.position);

        yield return new WaitForSeconds(0.3f);
        agent.stoppingDistance = 7;*/
        if (anim.GetInteger("Weapon Type") == 9)
        {
            shield = true;
        }
        yield return new WaitForSeconds(cooldown + UnityEngine.Random.Range(-1, 2));
        if (anim.GetInteger("Weapon Type") == 9)
        {
            shield = false;
        }

        agent.stoppingDistance = stopDist;
        canAtk = true;
        FindNearestPlayer();
    }

    IEnumerator RotateSaw()
    {
        GameObject saw = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Saw").transform.Find("Blade").gameObject;
        float topSpeed = 10;
        float currentSpeed = 0;
        float acceleration = 1f;
        while (agent.stoppingDistance != 7)
        {
            if (currentSpeed < topSpeed)
            {
                currentSpeed += acceleration;
            }
            saw.transform.Rotate(Vector3.right * currentSpeed);

            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitUntil(() => agent.stoppingDistance == 7);
    }

    public GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float minDistance = float.MaxValue;
        GameObject playerTarget = null;

        foreach (GameObject player in players)
        {
            float currDistance = Vector3.Distance(transform.position, player.transform.position);
            if (currDistance < minDistance)
            {
                playerTarget = player;
                minDistance = currDistance;
            }
        }
        /*if (playerTarget != null)
        {
            target = playerTarget.transform;
        }*/
        return playerTarget;
    }
    

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.Find("Root").transform.Find("Hips").position, directionToTarget, distanceToTarget, obstructionMask))
                {

                    canSeePlayer = true;
                    this.target = playerRef.transform;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

}

