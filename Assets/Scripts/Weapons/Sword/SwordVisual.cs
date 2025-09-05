using UnityEngine;

public class SwordVisual : MonoBehaviour
{
	private static readonly int AttackLightHash = Animator.StringToHash(AttackLight);
	private static readonly int AttackStrong = Animator.StringToHash(AttackSrtong);
	private static readonly int Death = Animator.StringToHash(SwordDeath);

	[SerializeField] private Sword sword;
	[SerializeField] private Player player;
	
	private const string AttackLight = "AttackLight";
	private const string AttackSrtong = "AttackStrong";
	private const string SwordDeath =  "SwordDeath";
	
	private Animator _animator;

	private bool _canQueueNextAttack;
	private bool _queueNextAttack;
	
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void Start()
	{
		player.OnPlayerDeath += PlayerOnOnPlayerDeath;
		sword.OnSwordSwing += Sword_OnSwordSwing;
	}

	private void PlayerOnOnPlayerDeath()
	{
		_animator.SetTrigger(Death);
	}

	private void Sword_OnSwordSwing(Sword.SwordAttackType type)
	{
		if (type == Sword.SwordAttackType.Light)
		{
			_animator.SetTrigger(AttackLightHash);
		}
		else if (type == Sword.SwordAttackType.Strong)
		{
			_animator.SetTrigger(AttackStrong);
		}
	}

	public void TriggerEndWindowCombo()
	{
		_canQueueNextAttack = false;
	}

	public void TriggerEndAttackAnimation()
	{
		sword.AttackColliderTurnOff();
	}

	private void OnDestroy()
	{
		sword.OnSwordSwing -= Sword_OnSwordSwing;
	}
}
