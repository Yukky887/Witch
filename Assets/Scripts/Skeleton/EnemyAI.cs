using System;
using UnityEngine;
using UnityEngine.AI;
using WildBerry.Utils;

public class EnemyAI : MonoBehaviour
{
	[SerializeField] private State startingState;
	[SerializeField] private float roamingDistanceMax = 7f;
	[SerializeField] private float roamingDistanceMin = 3f;
	[SerializeField] private float roamingTimerMax = 2f;
	[SerializeField] private bool isChasing;
	[SerializeField] private float chasingDistance = 5f;
	[SerializeField] private float chasingSpeedMultiplier = 2f;
	[SerializeField] private bool isAttackingEnemy;
	[SerializeField] private float attackRete = 2f;
	
	public Action OnEnemyAttack;
	
	private Vector3 _lastPosition;
	private Vector3 _startingPosition;
	private Vector3 _roamPosition;
	private NavMeshAgent _navMeshAgent;
	private State _currentState;
	
	private float _roamingTime;
	private float _roamingSpeed;
	private float _chasingSpeed;
	private float _nextAttackTime;
	private float _nextCheckDirectionTime;

	private const float AttackDistance = 2f;
	private const float CheckDirectionDuration = 0.1f;
	
	private void Awake()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.updateRotation = false;
		_navMeshAgent.updateUpAxis = false;
		_currentState = startingState;
		_roamingSpeed = _navMeshAgent.speed;
		_chasingSpeed = _navMeshAgent.speed * chasingSpeedMultiplier;
	}

	private void Update()
	{
		StateHandler();
		MovementDirectionHandler();
	}
	
	public bool IsRunning => _navMeshAgent.velocity != Vector3.zero;

	public void SetDeadState()
	{
		_navMeshAgent.ResetPath();
		_currentState = State.Dead;
		_navMeshAgent.ResetPath();
	}
	
	public float GetRoamingAnimationSpeed()
	{
		return _navMeshAgent.speed / _roamingSpeed;
	}
	
	private void StateHandler()
	{
		switch (_currentState)
		{
			case State.Roaming:
				_roamingTime -= Time.deltaTime;
				if (_roamingTime < 0)
				{
					Roaming();
					_roamingTime = roamingTimerMax;
				}

				CheckCurrentState();
				break;
			case State.Chasing:
				CasingTarget();
				CheckCurrentState();
				break;
			case State.Attacking:
				AttackingTarget();
				CheckCurrentState();
				break;
			case State.Dead:
				break;
			default:
			case State.Idle:
				CheckCurrentState();

				break;
		}
	}
	
	private void CheckCurrentState()
	{
		var distanceToPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);

		var newState = State.Roaming;

		if (isChasing && distanceToPlayer <= chasingDistance)
		{
			newState = State.Chasing;
		}

		if (isAttackingEnemy && distanceToPlayer <= AttackDistance)
		{
			newState = Player.Instance.IsAlive() ? State.Attacking : State.Roaming;
		}

		if (newState != _currentState)
		{
			if (newState == State.Chasing)
			{
				_navMeshAgent.speed = _chasingSpeed;
				_navMeshAgent.ResetPath();
			}

			if (newState == State.Roaming)
			{
				_roamingTime = 0f;
				_navMeshAgent.speed = _roamingSpeed;
			}

			if (newState == State.Attacking)
			{
				_navMeshAgent.ResetPath();
			}

			_currentState = newState;
		}
		
	}
	
	private void CasingTarget()
	{
		_navMeshAgent.SetDestination(Player.Instance.transform.position);
	}
	
	private void AttackingTarget()
	{
		if (!(Time.time >= _nextAttackTime))
		{
			return;
		}
		
		OnEnemyAttack?.Invoke();

		_nextAttackTime = Time.time + attackRete;
	}

	private void MovementDirectionHandler()
	{
		if (!(Time.time > _nextCheckDirectionTime))
		{
			return;
		}
		
		if (IsRunning)
		{
			ChangeFacingDirection(_lastPosition, transform.position);
		}
		else if ( _currentState == State.Attacking)
		{
			ChangeFacingDirection(transform.position, Player.Instance.transform.position);
		}

		_lastPosition = transform.position;
		_nextCheckDirectionTime = Time.time + CheckDirectionDuration;

	}

	private void Roaming()
	{
		_startingPosition = transform.position;
		_roamPosition = GetRoamingPosition();
		_navMeshAgent.SetDestination(_roamPosition);
	}

	private Vector3 GetRoamingPosition()
	{
		return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(-roamingDistanceMin, roamingDistanceMax);
	}

	
	private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
	{
		transform.rotation = sourcePosition.x > targetPosition.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
	}
	
	private enum State
	{
		Idle,
		Roaming,
		Chasing,
		Attacking,
		Dead
	}
}
