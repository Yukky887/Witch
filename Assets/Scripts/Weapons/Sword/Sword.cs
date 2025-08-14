using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Sword : MonoBehaviour
{
	/// <summary>
	/// ������ ���� ����.
	/// </summary>
	[SerializeField] private int _liteHitDamage = 5;

	/// <summary>
	/// ������� ���� ����.
	/// </summary>
	[SerializeField] private int _strongHitDamage = 8;

	/// <summary>
	/// ������� ����� ����.
	/// </summary>
	private PolygonCollider2D _polygonCollider2D;

	/// <summary>
	/// ��������� PolygonCollider2D ��� ����������� ������� ����� ����.
	/// </summary>
	private void Awake()
	{
		_polygonCollider2D = GetComponent<PolygonCollider2D>();
	}

	/// <summary>
	/// ��������� ��������� ������� ����� ���� ��� ������ ����.
	/// </summary>
	private void Start()
	{
		AttackPoligonColliderTurnOff();
	}

	/// <summary>
	/// ���� ����� �����.
	/// </summary>
	public enum SwordAttackType
	{
		Light,
		Strong
	}

	/// <summary>
	/// �������, ���������� ��� ����� �����.
	/// </summary>
	public event Action<SwordAttackType> OnSwordSwing;


	/// <summary>
	/// ������� ����� � �������� ����� �����.
	/// </summary>
	/// <param name="attackType">��� ������.</param>
	public void Attack(SwordAttackType attackType)
	{
		AttackPoligonColliderTurnOn();

		OnSwordSwing?.Invoke(attackType);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out EnemiEntity enemiEntity))
		{
			enemiEntity.TakeDamage(_liteHitDamage);
		}
	}

	/// <summary>
	/// ��������� ��������� ������� ����� ���� ����� ���������� �����.
	/// </summary>
	public void AttackPoligonColliderTurnOff()
	{
		_polygonCollider2D.enabled = false;
	}

	/// <summary>
	/// �������� ��������� ������� ����� ���� ����� ������.
	/// </summary>
	private void AttackPoligonColliderTurnOn()
	{
		_polygonCollider2D.enabled = true;
	}
}
