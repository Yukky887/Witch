using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkeletonVisual : MonoBehaviour
{

	[SerializeField] private EnemyAI _enemyAI;
	[SerializeField] private EnemiEntity _enemyEntity;

	/// <summary>
	/// Подулючает компонент Animator для управления анимациями скелета.
	/// </summary>
	private Animator _animator;
	
	#region Константы

	/// <summary>
	/// Находится ли скелет в состоянии бега.
	/// </summary>
	private const string IS_RUNNING = "isRunning";

	/// <summary>
	/// Триггер для атаки скелета.
	/// </summary>
	private const string ATTACK = "Attack";

	/// <summary>
	/// Находится ли скелет в состоянии приследования.
	/// </summary>
	private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";

	#endregion

	#region Методы.

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		_enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack; ;
	}

	private void Update()
	{
		_animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
		_animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
	}

	/// <summary>
	/// Включает коллайдер при достижении определенного фрейма.
	/// </summary>
	public void TriggerAttackAnimationTurnOn()
	{
		_enemyEntity.SetPolygonColliderTurnOn();
	}

	/// <summary>
	/// Отключает коллайдер при достижении определенного фрейма.
	/// </summary>
	public void TriggerAttackAnimationTurnOff()
	{
		_enemyEntity.SetPolygonColliderTurnOff();
	}

	/// <summary>
	/// Вызывает триггер при атаке врага.
	/// </summary>
	private void _enemyAI_OnEnemyAttack()
	{
		_animator.SetTrigger(ATTACK);
	}

	/// <summary>
	/// Отписывается от события атаки врага.
	/// </summary>
	private void OnDestroy()
	{
		_enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;	
	}

	#endregion
}
