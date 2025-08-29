using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
	private static readonly int AttackHash = Animator.StringToHash(Attack);

	[SerializeField] private Sword.SwordAttackType swordAttackType;
	[SerializeField] private Sword sword;

	private const string Attack = "Attack";
	private Animator _animator;
	
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void Start()
	{
		sword.OnSwordSwing += OnSwordSwing;
	}
	
	private void OnSwordSwing(Sword.SwordAttackType attackType)
	{
		if (attackType != swordAttackType)
		{
			return;
		}

		_animator.SetTrigger(AttackHash);
	}
	
	private void OnDestroy()
	{
		sword.OnSwordSwing -= OnSwordSwing;
	}
}
