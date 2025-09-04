using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
	private static readonly int Die = Animator.StringToHash(IsDie);
	private static readonly int Hit = Animator.StringToHash(TakeHit);
	private static readonly int Running = Animator.StringToHash(IsRunning);
	private static readonly int SpeedMultiplier = Animator.StringToHash(ChasingSpeedMultiplier);
	private static readonly int Attack1 = Animator.StringToHash(Attack);

	[SerializeField] private EnemyAI enemyAI;
	[SerializeField] private EnemiEntity enemyEntity;
	[SerializeField] private GameObject enemyShadow;


	private Animator _animator;
	
	private const string IsRunning = "isRunning";
	private const string Attack = "Attack";
	private const string TakeHit = "TakeHit";
	private const string IsDie = "isDie";
	private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";


	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_animator = GetComponent<Animator>();

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;

		enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;

		enemyEntity.OnDie += _enemyEntity_OnDie;
	}

	private void _enemyEntity_OnDie()
	{
		_animator.SetBool(Die, true);
		_spriteRenderer.sortingOrder = -1;
		enemyShadow.SetActive(false);
		Debug.Log("Enemy has died and AI is disabled.");
	}

	private void _enemyEntity_OnTakeHit()
	{
		_animator.SetTrigger(Hit);
	}

	private void Update()
	{
		_animator.SetBool(Running, enemyAI.IsRunning);
		_animator.SetFloat(SpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
	}
	
	public void TriggerAttackAnimationTurnOn()
	{
		enemyEntity.SetPolygonColliderTurnOn();
	}
	
	public void TriggerAttackAnimationTurnOff()
	{
		enemyEntity.SetPolygonColliderTurnOff();
	}
	
	private void _enemyAI_OnEnemyAttack()
	{
		_animator.SetTrigger(Attack1);
	}
	
	private void OnDestroy()
	{
		enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;	
		enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
		enemyEntity.OnDie -= _enemyEntity_OnDie;
	}
}
