using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
	[SerializeField] private int liteHitDamage = 5;
	[SerializeField] private int strongHitDamage = 8;
	
	private PolygonCollider2D _polygonCollider2D;
	
	private void Awake()
	{
		_polygonCollider2D = GetComponent<PolygonCollider2D>();
	}

	private void Start()
	{
		AttackColliderTurnOff();
	}

	public enum SwordAttackType
	{
		Light,
		Strong
	}
	
	public event Action<SwordAttackType> OnSwordSwing;
	
	public void Attack(SwordAttackType attackType)
	{
		AttackColliderTurnOn();

		OnSwordSwing?.Invoke(attackType);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out EnemiEntity enemiEntity))
		{
			enemiEntity.TakeDamage(liteHitDamage);
		}
	}
	
	public void AttackColliderTurnOff()
	{
		_polygonCollider2D.enabled = false;
	}
	
	private void AttackColliderTurnOn()
	{
		_polygonCollider2D.enabled = true;
	}
}
