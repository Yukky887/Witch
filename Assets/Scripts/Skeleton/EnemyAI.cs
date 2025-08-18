using System;
using UnityEngine;
using UnityEngine.AI;
using WildBerry.Utils;

public class EnemyAI : MonoBehaviour
{
	#region Компоненты и переменные
	/// <summary>
	/// Cостояние врага при запуске игры.
	/// </summary>
	[SerializeField] private State _startingState;

	/// <summary>
	/// Максимальная дистанция для блуждания врага.
	/// </summary>
	[SerializeField] private float _roamingDistanceMax = 7f;

	/// <summary>
	/// Минимальная дистанция для блуждания врага.
	/// </summary>
	[SerializeField] private float _roamingDistanceMin = 3f;

	/// <summary>
	/// Максимальное время, которое враг может блуждать в одном направлении.
	/// </summary>
	[SerializeField] private float _roamingTimerMax = 2f;

	/// <summary>
	/// Приследуюущий ли враг.
	/// </summary>
	[SerializeField] private bool _isCheasing = false;

	/// <summary>
	/// Расстояние, на котором враг начинает преследовать игрока.
	/// </summary>
	[SerializeField] private float _chasingDistance = 5f;

	/// <summary>
	/// Множитель скорости врага при преследовании.
	/// </summary>
	[SerializeField] private float _chasingSpeedMultiplier = 2f;

	/// <summary>
	/// Является ли враг атакующим.
	/// </summary>
	[SerializeField] private bool _isAttackingEnemy = false;

	/// <summary>
	/// Множитель частоты атак врага.
	/// </summary>
	[SerializeField] private float _attackRete = 2f;


	/// <summary>
	/// Точка навигации для врага.
	/// </summary>
	private NavMeshAgent _navMeshAgent;

	/// <summary>
	/// Расстояние, на котором враг может атаковать игрока
	/// </summary>
	private float _attackDistance = 2f;

	/// <summary>
	/// Текущее состояние врага.
	/// </summary>
	private State _currentState;
	private float _roamingTime;
	private Vector3 _roamPosition;
	private Vector3 _startingPosition;

	/// <summary>
	/// Скорость хождения врага.
	/// </summary>
	private float _roamingSpeed;

	/// <summary>
	/// Скорость врага при преследовании.
	/// </summary>
	private float _сhasingSpeed;

	/// <summary>
	/// Время между атаками врага.
	/// </summary>
	private float _nextAttackTime = 0f;

	/// <summary>
	/// Время проверки направления движения врага.
	/// </summary>
	private float _nextCheckDirctionTime = 0f;

	/// <summary>
	/// Чат для проверки направления движения врага.
	/// </summary>
	private float _checkDirectionDuration = 0.1f;

	/// <summary>
	/// Последняя позиция врага.
	/// </summary>
	private Vector3 _lastPosition;

	#endregion




	#region Методы
	private void Awake()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.updateRotation = false;
		_navMeshAgent.updateUpAxis = false;
		_currentState = _startingState;
		_roamingSpeed = _navMeshAgent.speed;
		_сhasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
	}

	private void Update()
	{
		StateHandler();
		MovementDurectionHandler();
	}

	/// <summary>
	/// Скорость анимации блуждания врага.
	/// </summary>
	/// <returns>Скорость блуждания.</returns>
	public float GetRoamingAnimationSpeed()
	{
		return _navMeshAgent.speed / _roamingSpeed;
	}

	/// <summary>
	/// Выбор состояния врага и выполнение соответствующих действий.
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
	/// Проверяет и изменяет текущий статус врага.
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
			newState = State.Attacking;
		}

		if (newState != _currentState)
		{
			if (newState == State.Chasing)
			{
				_navMeshAgent.speed = _сhasingSpeed;
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
	/// Цель преследования врага.
	/// </summary>
	private void CasingTarget()
	{
		_navMeshAgent.SetDestination(Player.Instance.transform.position);
	}

	/// <summary>
	/// Враг атакует цель.
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
	/// Разворачивает врага в сторону цели.
	/// </summary>
	/// <param name="sourcePosition">Изначальное положение.</param>
	/// <param name="targetPosition">Последнее положение.</param>
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
	/// Состояния врага в игре.
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

	#region Свойства

	/// <summary>
	/// Бежит ли враг в данный момент.
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
	
	#region События Unity

	public event Action OnEnemyAttack;

	#endregion
}
