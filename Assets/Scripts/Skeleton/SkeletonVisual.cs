using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{

	[SerializeField] private EnemyAI _enemyAI;
	[SerializeField] private EnemiEntity _enemyEntity;
	[SerializeField] private GameObject _enemyShadow;

	/// <summary>
	/// ���������� ��������� Animator ��� ���������� ���������� �������.
	/// </summary>
	private Animator _animator;
	
	#region ���������

	/// <summary>
	/// ��������� �� ������ � ��������� ����.
	/// </summary>
	private const string IS_RUNNING = "isRunning";

	/// <summary>
	/// ������� ��� ����� �������.
	/// </summary>
	private const string ATTACK = "Attack";

	/// <summary>
	/// ������� ��� ��������� ����� ��������.
	/// </summary>
	private const string TAKE_HIT = "TakeHit";

	/// <summary>
	/// ������� ��� ������ �������.
	/// </summary>
	private const string IS_DIE = "isDie";

	/// <summary>
	/// ��������� �� ������ � ��������� �������������.
	/// </summary>
	private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";


	SpriteRenderer _spriteRenderer;

	#endregion

	#region ������.

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
		_spriteRenderer.sortingOrder = -1; // ������ ������� ���������, ����� ������ �� ���������� ������ �������
		_enemyShadow.SetActive(false); // ��������� ����, ���� ��� ����
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
	/// �������� ��������� ��� ���������� ������������� ������.
	/// </summary>
	public void TriggerAttackAnimationTurnOn()
	{
		_enemyEntity.SetPolygonColliderTurnOn();
	}

	/// <summary>
	/// ��������� ��������� ��� ���������� ������������� ������.
	/// </summary>
	public void TriggerAttackAnimationTurnOff()
	{
		_enemyEntity.SetPolygonColliderTurnOff();
	}

	/// <summary>
	/// �������� ������� ��� ����� �����.
	/// </summary>
	private void _enemyAI_OnEnemyAttack()
	{
		_animator.SetTrigger(ATTACK);
	}

	/// <summary>
	/// ������������ �� ������� ����� �����.
	/// </summary>
	private void OnDestroy()
	{
		_enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;	
	}

	#endregion
}
