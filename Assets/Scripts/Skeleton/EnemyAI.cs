using System;
using UnityEngine;
using UnityEngine.AI;
using WildBerry.Utils;

public class EnemyAI : MonoBehaviour
{
	/// <summary>
	/// C�������� ����� ��� ������� ����.
	/// </summary>
	[SerializeField] private State _startingState;

	/// <summary>
	/// ������������ ��������� ��� ��������� �����.
	/// </summary>
	[SerializeField] private float _roamingDistanceMax = 7f;

	/// <summary>
	/// ����������� ��������� ��� ��������� �����.
	/// </summary>
	[SerializeField] private float _roamingDistanceMin = 3f;

	/// <summary>
	/// ������������ �����, ������� ���� ����� �������� � ����� �����������.
	/// </summary>
	[SerializeField] private float _roamingTimerMax = 2f;

	/// <summary>
	/// ������������� �� ����.
	/// </summary>
	[SerializeField] private bool _isCheasing = false;

	/// <summary>
	/// ����������, �� ������� ���� �������� ������������ ������.
	/// </summary>
	[SerializeField] private float _chasingDistance = 5f;

	/// <summary>
	/// ��������� �������� ����� ��� �������������.
	/// </summary>
	[SerializeField] private float _chasingSpeedMultiplier = 2f;

	/// <summary>
	/// �������� �� ���� ���������.
	/// </summary>
	[SerializeField] private bool _isAttackingEnemy = false;

	/// <summary>
	/// ��������� ������� ���� �����.
	/// </summary>
	[SerializeField] private float _attackRete = 2f;


	/// <summary>
	/// ����� ��������� ��� �����.
	/// </summary>
	private NavMeshAgent _navMeshAgent;

	/// <summary>
	/// ����������, �� ������� ���� ����� ��������� ������
	/// </summary>
	private float _attackDistance = 2f;

	/// <summary>
	/// ������� ��������� �����.
	/// </summary>
	private State _currentState;
	private float _roamingTime;
	private Vector3 _roamPosition;
	private Vector3 _startingPosition;

	/// <summary>
	/// �������� �������� �����.
	/// </summary>
	private float _roamingSpeed;

	/// <summary>
	/// �������� ����� ��� �������������.
	/// </summary>
	private float _chasingSpeed;

	/// <summary>
	/// ����� ����� ������� �����.
	/// </summary>
	private float _nextAttackTime = 0f;

	/// <summary>
	/// ����� �������� ����������� �������� �����.
	/// </summary>
	private float _nextCheckDirctionTime = 0f;

	/// <summary>
	/// ��� ��� �������� ����������� �������� �����.
	/// </summary>
	private float _checkDirectionDuration = 0.1f;

	/// <summary>
	/// ��������� ������� �����.
	/// </summary>
	private Vector3 _lastPosition;

	#region ������
	private void Awake()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.updateRotation = false;
		_navMeshAgent.updateUpAxis = false;
		_currentState = _startingState;
		_roamingSpeed = _navMeshAgent.speed;
		_chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
	}

	private void Update()
	{
		StateHandler();
		MovementDurectionHandler();
	}
	
	public void SetDeadState()
	{
		_navMeshAgent.ResetPath();
		_currentState = State.Dead;
		_navMeshAgent.ResetPath();
	}

	/// <summary>
	/// �������� �������� ��������� �����.
	/// </summary>
	/// <returns>�������� ���������.</returns>
	public float GetRoamingAnimationSpeed()
	{
		return _navMeshAgent.speed / _roamingSpeed;
	}

	/// <summary>
	/// ����� ��������� ����� � ���������� ��������������� ��������.
	/// </summary>
	private void StateHandler()
	{
		switch (_currentState)
		{
			case State.Roaming:
				_roamingTime -= Time.deltaTime;
				if (_roamingTime < 0)
				{
					Roaming();
					_roamingTime = _roamingTimerMax;
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

	/// <summary>
	/// ��������� � �������� ������� ������ �����.
	/// </summary>
	private void CheckCurrentState()
	{
		var distanceToPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);

		State newState = State.Roaming;

		if (_isCheasing && distanceToPlayer <= _chasingDistance)
		{
			newState = State.Chasing;
		}

		if (_isAttackingEnemy && distanceToPlayer <= _attackDistance)
		{
			if (Player.Instance.IsAlive())
			{
				newState = State.Attacking;
			}
			else
			{
				newState = State.Roaming;
			}
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

	/// <summary>
	/// ���� ������������� �����.
	/// </summary>
	private void CasingTarget()
	{
		_navMeshAgent.SetDestination(Player.Instance.transform.position);
	}

	/// <summary>
	/// ���� ������� ����.
	/// </summary>
	private void AttackingTarget()
	{
		if (Time.time >= _nextAttackTime)
		{
			OnEnemyAttack?.Invoke();

			_nextAttackTime = Time.time + _attackRete;
		}
	}

	private void MovementDurectionHandler()
	{
		if (Time.time > _nextCheckDirctionTime)
		{
			if (IsRunning)
			{
				ChangeFacingDirection(_lastPosition, transform.position);
			}
			else if ( _currentState == State.Attacking)
			{
				ChangeFacingDirection(transform.position, Player.Instance.transform.position);
			}

			_lastPosition = transform.position;
			_nextCheckDirctionTime = Time.time + _checkDirectionDuration;
		}
		
	}

	private void Roaming()
	{
		_startingPosition = transform.position;
		_roamPosition = GetRoamingPosition();
		_navMeshAgent.SetDestination(_roamPosition);
	}

	private Vector3 GetRoamingPosition()
	{
		return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(-_roamingDistanceMin, _roamingDistanceMax);
	}


	/// <summary>
	/// ������������� ����� � ������� ����.
	/// </summary>
	/// <param name="sourcePosition">����������� ���������.</param>
	/// <param name="targetPosition">��������� ���������.</param>
	private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
	{
		if (sourcePosition.x > targetPosition.x)
		{
			transform.rotation = Quaternion.Euler(0, -180, 0);
		}
		else
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	/// <summary>
	/// ��������� ����� � ����.
	/// </summary>
	private enum State
	{
		Idle,
		Roaming,
		Chasing,
		Attacking,
		Dead
	}

	#endregion

	#region ��������

	/// <summary>
	/// ����� �� ���� � ������ ������.
	/// </summary>
	public bool IsRunning
	{
		get
		{

			if (_navMeshAgent.velocity == Vector3.zero)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}


	#endregion
	
	#region ������� Unity

	public event Action OnEnemyAttack;

	#endregion
}
