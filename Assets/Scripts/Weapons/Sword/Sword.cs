using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Sword : MonoBehaviour
{
	/// <summary>
	/// Слабый урон меча.
	/// </summary>
	[SerializeField] private int _liteHitDamage = 5;

	/// <summary>
	/// Сильный урон меча.
	/// </summary>
	[SerializeField] private int _strongHitDamage = 8;

	/// <summary>
	/// Область атаки меча.
	/// </summary>
	private PolygonCollider2D _polygonCollider2D;

	/// <summary>
	/// Компонент PolygonCollider2D для определения области атаки меча.
	/// </summary>
	private void Awake()
	{
		_polygonCollider2D = GetComponent<PolygonCollider2D>();
	}

	/// <summary>
	/// Выключает коллайдер области атаки меча при старте игры.
	/// </summary>
	private void Start()
	{
		AttackPoligonColliderTurnOff();
	}

	/// <summary>
	/// Виды атаки мечом.
	/// </summary>
	public enum SwordAttackType
	{
		Light,
		Strong
	}

	/// <summary>
	/// Событие, вызываемое при атаке мечом.
	/// </summary>
	public event Action<SwordAttackType> OnSwordSwing;


	/// <summary>
	/// Атакует мечом с заданным типом атаки.
	/// </summary>
	/// <param name="attackType">Тип аттаки.</param>
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
	/// Выключает коллайдер области атаки меча после завершения атаки.
	/// </summary>
	public void AttackPoligonColliderTurnOff()
	{
		_polygonCollider2D.enabled = false;
	}

	/// <summary>
	/// Включает коллайдер области атаки меча перед атакой.
	/// </summary>
	private void AttackPoligonColliderTurnOn()
	{
		_polygonCollider2D.enabled = true;
	}
}
