using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(BaseCharacter))]
public class AICharacter : MonoBehaviour
{
	public enum WeaponTypes
	{
		SwordAndShield,
		SingleSwordA,
		SingleSwordB,
		TwoSwords,
		GiantHammer,
		Staff,
		Axe,
		MartialBrute,
		Magic
		
	}

	public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
	public BaseCharacter character { get; private set; } // the character we are controlling
	public int AI_ID;//the id of this AI character
	[SerializeField] WeaponTypes weaponType;
	public Transform target;                                    // target to aim for
	private Animator anim;

	[SerializeField] int cooldown;//this is the cooldown for the AI character after a stamina loss


	private string enemy;


	public bool canAtk = true;
	private int atkCount = 0;
	private int basicAtks = 0;
	private float stopDist = 0;
	private float speed = 0;




	private bool basicAtk = false;
	private bool shield = false;

	public float radius;
	[Range(0, 360)]
	public float angle;

	public GameObject targetRef;

	public LayerMask targetMask;
	public LayerMask obstructionMask;

	//public bool canSeeTarget;

	private void Start()
	{
		// get the components on the object we need ( should not be null due to require component so no need to check )
		agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
		character = GetComponent<BaseCharacter>();

		agent.updateRotation = true;
		agent.updatePosition = true;
		targetRef = FindNearestTarget(!gameObject.CompareTag("Enemy"));
		if (targetRef != null)
		{
			target = targetRef.transform;
		}
		StartCoroutine(FOVRoutine());

		anim = GetComponent<Animator>();
		anim.SetInteger("AI_ID", AI_ID);
		stopDist = agent.stoppingDistance;
		speed = agent.speed;
		anim.SetInteger("Weapon Type", (int)weaponType);

		switch (weaponType)
		{
			case WeaponTypes.SwordAndShield:
				basicAtks = 3;
				break;
			case WeaponTypes.SingleSwordA:
				basicAtks = 4;
				break;
			case WeaponTypes.SingleSwordB:
				basicAtks = 4;
				break;
			case WeaponTypes.TwoSwords:
				basicAtks = 3;
				break;
			case WeaponTypes.GiantHammer:
				basicAtks = 3;
				break;
			case WeaponTypes.Staff:
				basicAtks = 2;
				break;
			case WeaponTypes.Axe:
				basicAtks = 3;
				break;
			case WeaponTypes.MartialBrute:
				basicAtks = 3;
				break;
			case WeaponTypes.Magic:
				basicAtks = 2;
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
			}
			else
			{
				agent.stoppingDistance = stopDist;
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
					if (AI_ID == 9 && weaponType == WeaponTypes.Axe)
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
		targetRef = target.gameObject;
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


		anim.SetInteger("Random Atk Val", 0);
		canAtk = false;


		if (weaponType == WeaponTypes.SwordAndShield)
		{
			shield = true;
		}
		yield return new WaitForSeconds(cooldown + UnityEngine.Random.Range(-1, 2));
		if (weaponType == WeaponTypes.SwordAndShield)
		{
			shield = false;
		}

		agent.stoppingDistance = stopDist;
		canAtk = true;
        targetRef = FindNearestTarget(!gameObject.CompareTag("Enemy"));
        if (targetRef != null)
        {
            target = targetRef.transform;
        }
    }

	IEnumerator RotateSaw()
	{
		GameObject saw = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Mech Saw").transform.Find("Blade").gameObject;
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

	public GameObject FindNearestTarget(bool targetEnemies)
	{

		GameObject[] targets;
		if (targetEnemies)
		{
			targets = GameObject.FindGameObjectsWithTag("Enemy");
		} else
		{
			GameObject[] playerTargets = GameObject.FindGameObjectsWithTag("Player");
			GameObject[] allyTargets = GameObject.FindGameObjectsWithTag("Ally");

			targets = new GameObject[allyTargets.Length + playerTargets.Length];
			playerTargets.CopyTo(targets, 0);
			allyTargets.CopyTo(targets, playerTargets.Length);
			
		}
		 
		float minDistance = float.MaxValue;
		GameObject targetObj = null;

		foreach (GameObject target in targets)
		{
			float currDistance = Vector3.Distance(transform.position, target.transform.position);
			if (currDistance < minDistance && FieldOfViewCheck(target))
			{
				targetObj = target;
				minDistance = currDistance;
			}
		}
		return targetObj;
	}
	

	private IEnumerator FOVRoutine()
	{
		WaitForSeconds wait = new WaitForSeconds(0.2f);

		while (true)
		{
			yield return wait;
			if (targetRef != null)
			{
				if (!FieldOfViewCheck(targetRef))
				{
					yield return new WaitForSeconds(3);
					if (!FieldOfViewCheck(targetRef))
					{
						target = null;
						targetRef = null;
						Debug.Log("chilly");
						StartCoroutine(CoolAtk());
					}
				}
			} else
			{
                yield return new WaitForSeconds(3);
                StartCoroutine(CoolAtk());

            }
        }
	}

	private bool FieldOfViewCheck(GameObject obj)
	{
		bool canSeeTarget = false;
		Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
		if (rangeChecks.Length != 0 && Array.IndexOf(rangeChecks, obj.GetComponent<Collider>()) != -1)
		{
			int index = Array.IndexOf(rangeChecks, obj.GetComponent<Collider>());

			Transform target = rangeChecks[index].transform;
			Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
			{

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.Find("Root").transform.Find("Hips").position, directionToTarget, distanceToTarget, obstructionMask))
				{

                    canSeeTarget = true;
					
				}
				else
				{
					canSeeTarget = false;
				}
			}
			else
			{
				canSeeTarget = false;
			}
		}
		else if (canSeeTarget)
		{
			canSeeTarget = false;
		}
		return canSeeTarget;
	}



}

