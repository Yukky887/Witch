using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkeletonVisual : MonoBehaviour
{

	[SerializeField] private EnemyAI _enemyAI;
	[SerializeField] private EnemiEntity _enemyEntity;

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
	/// ��������� �� ������ � ��������� �������������.
	/// </summary>
	private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";

	#endregion

	#region ������.

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
