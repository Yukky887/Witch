using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{

	[SerializeField] private EnemyAI _enemyAI;
	[SerializeField] private EnemiEntity _enemyEntity;
	[SerializeField] private GameObject _enemyShadow;

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
	/// Триггер для получения урона скелетом.
	/// </summary>
	private const string TAKE_HIT = "TakeHit";

	/// <summary>
	/// Триггер для смерти скелета.
	/// </summary>
	private const string IS_DIE = "isDie";

	/// <summary>
	/// Находится ли скелет в состоянии приследования.
	/// </summary>
	private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";


	SpriteRenderer _spriteRenderer;

	#endregion

	#region Методы.

	private void Awake()
	{
		_animator = GetComponent<Animator>();

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		_enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;

		_enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;

		_enemyEntity.OnDie += _enemyEntity_OnDie;
	}

	private void _enemyEntity_OnDie()
	{
		_animator.SetBool(IS_DIE, true);
		_spriteRenderer.sortingOrder = -1; // Меняем порядок отрисовки, чтобы скелет не перекрывал другие объекты
		_enemyShadow.SetActive(false); // Отключаем тень, если она есть
		Debug.Log("Enemy has died and AI is disabled.");
	}

	private void _enemyEntity_OnTakeHit()
	{
		_animator.SetTrigger(TAKE_HIT);
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
