using UnityEngine;

public class SwordVisual : MonoBehaviour
{
	private static readonly int AttackLightHash = Animator.StringToHash(AttackLight);
	private static readonly int AttackStrong = Animator.StringToHash(AttackSrtong);
	[SerializeField] private Sword sword;
	
	private Animator _animator;

	private const string AttackLight = "AttackLight";
	private const string AttackSrtong = "AttackStrong";


	private bool _canQueueNextAttack;
	private bool _queueNextAttack;
	
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void Start()
	{
		sword.OnSwordSwing += Sword_OnSwordSwing;
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
