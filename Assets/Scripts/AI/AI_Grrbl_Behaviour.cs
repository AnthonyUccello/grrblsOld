using UnityEngine;
using System.Collections;
using Pathfinding;


/*
 * Handles delivering damage
 * Handles triggering death
 * Handles animations
 * Handles targetting
 * 
 * Goes on Grrbl prefab
 * */
public class AI_Grrbl_Behaviour : MonoBehaviour {

	//todo
	//Set target in range to NULL if target is null

	public Transform raycaster;
	public float speed = 1;
	public float nextWayPointDistance = 3f;
	public Transform endDestination;

	private GameObject target;
	private Seeker _seeker;
	private Path _path;
	private int _currentWayPoint = 0;
	private float _lastPathCalculationTime;
	private float _pathRecalculationInterval = 0.3f;
	private float _lastAttackTime = 0;
	private bool _isAttacking = false;
	private Manager_Grrbl_Stats selfStats;
	bool _resetAttackTime = true;

	private int _layerMaskSelf;//set it to ignore its own layer type
	private int _layerMaskGround = 1<<8;
	private int _layerMaskTotal;
	//animator stuff
	private Animator _animator;
	private AnimatorStateInfo _animatorStateInfo;

	int isWalkingHash = Animator.StringToHash("isWalking");
	int _sAttack_H = Animator.StringToHash("Base Layer.Attack_H");
	
	public void init()
	{
		_animator = GetComponent<Animator> ();
		_seeker = GetComponent<Seeker> ();
		_seeker.pathCallback+= onPathComplete;
		StartCoroutine(RecalculatePath());
		_layerMaskSelf = 1<<gameObject.layer;
		selfStats = transform.GetComponent<Manager_Grrbl_Stats>();
		_layerMaskTotal = ~(_layerMaskGround | _layerMaskSelf);
	}
	
	// Update is called once per frame
	void Update () {
		if(target==null || target.name == endDestination.name)
		{
			scanForNewTarget();
		}

		_animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
	}

	//raycast
	void scanForNewTarget()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(raycaster.position,fwd*1000,Color.green);
		RaycastHit hit;
		if(Physics.Raycast(raycaster.position,transform.forward,out hit,1000,_layerMaskTotal))
		{
			target = hit.transform.gameObject;
		}else
		{
			//no target found set it to end destination
			target=endDestination.gameObject;
		}
	}

	//called when path calculation is complete (NOT PATH TRAVERSING).
	void onPathComplete(Path p)
	{
		if (!p.error) {
			_path = p;
			_currentWayPoint = 0;
			if(target!=null)
			{
				transform.LookAt(target.transform.position);
			}else
			{
				transform.LookAt(endDestination.position);
			}
		}

		_lastPathCalculationTime = Time.time;
	}

	//its only doing MELEE right now
	void processAttack()
	{
		if(_resetAttackTime)
		{
			_lastAttackTime = Time.time;
			_resetAttackTime = false;
		}

		if(selfStats.killed)
		{
			_animator.SetBool("isAttacking",false);
			return;
		}

		if(target==null || target.GetComponent<Manager_Grrbl_Stats>()==null)
		{
			_isAttacking = false;
			_resetAttackTime = true;
			_animator.SetBool("isAttacking",false);
			_animator.Play("Idle");
			return;
		}

		if(target.GetComponent<Manager_Grrbl_Stats>().killed)
		{
			_isAttacking=false;
			_resetAttackTime = true;
			_animator.SetBool("isAttacking",false);
			_animator.Play ("Idle");
			return;
		}

		if(_animatorStateInfo.nameHash != _sAttack_H)
		{
			_animator.SetBool("isWalking",false);
		}

		if(Time.time - _lastAttackTime > selfStats.attackSpeed && !target.GetComponent<Manager_Grrbl_Stats>().killed)
		{
			_lastAttackTime = Time.time;
			target.GetComponent<Manager_Grrbl_Stats>().acceptAttack(selfStats.attackPayload);
		}
	}

	public void FixedUpdate()
	{
	
		if(_path==null)
		{
		
		//	Debug.Log("Null path");
			return;
		}

		if(_currentWayPoint >= _path.vectorPath.Count)
		{
			//grrbl has reached the end, deduct player health
			if(Vector3.Distance(transform.position,endDestination.position)<nextWayPointDistance)
			{
				//todo: deduct health
				if(gameObject.tag==Enum_Tags.AI_GRRL)
				{
					Overseer_PlayerHealth.reducePlayerHealth();
				}else
				{
					Overseer_PlayerHealth.reduceAIHealth();
				}
				Destroy(gameObject);
				_seeker.pathCallback-=onPathComplete;
			}

			return;
		}
		//if its not the end point then check distance to target
		if(target!=null)
		{
			if(target.tag==Enum_Tags.AI_GRRL || target.tag == Enum_Tags.PLAYER_GRRBL)
			{
				if(Vector3.Distance(target.transform.position,transform.position)<nextWayPointDistance)
				{
					_isAttacking = true;
					_animator.SetBool("isAttacking",true);
					_animator.SetBool("isWalking",false);
					processAttack();
					return; //return here so we don't advance to the next waypoint
				}
			}
		}

		processWalking();
	}
	

	void processWalking()
	{
		_animator.SetBool("isAttacking",false);
		_animator.SetBool("isWalking",true);

		Vector3 dir = (_path.vectorPath[_currentWayPoint] - transform.position).normalized;
		transform.position += dir*Time.fixedDeltaTime * speed;
		if(Vector3.Distance(transform.position,_path.vectorPath[_currentWayPoint]) < nextWayPointDistance)
		{
			_currentWayPoint++;
		}
	}

	IEnumerator RecalculatePath()
	{
		while(true)
		{
			//check if target is null
			if(target==null && Time.time - _lastPathCalculationTime > _pathRecalculationInterval)
			{
				_seeker.StartPath(transform.position,endDestination.position);
				_lastPathCalculationTime = Time.time;
			}else if(Time.time - _lastPathCalculationTime > _pathRecalculationInterval && !_isAttacking)
			{
				_seeker.StartPath(transform.position,target.transform.position);
				_lastPathCalculationTime = Time.time;
			}
			yield return 0;

		}
	}

	public void killSelf()
	{
		_animator.SetBool("isAttacking",false);
		_animator.SetBool("isDead",true);
		StartCoroutine(processKillSelf());
	}

	IEnumerator processKillSelf()
	{
		yield return new WaitForSeconds(1f);//its a 30 frame animation
		Destroy(gameObject);
	}
}
