using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ссылаемся на объект, который должен быть добавлен в инспекторе Unity.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class EnemiEntity : MonoBehaviour
{
	[SerializeField] private int _maxHealth;

	private int _currentHealth;

	private PolygonCollider2D _collider;

	private void Awake()
	{
		_collider = GetComponent<PolygonCollider2D>();
	}

	private void Start()
	{
		_currentHealth = _maxHealth;
	}

	/// <summary>
	/// Обрабатывает пересечение коллайдера меча скелета с другими.
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log($"Collision with {collision.gameObject.name} detected.");
	}

	/// <summary>
	/// Выключает коллайдер.
	/// </summary>
	public void SetPolygonColliderTurnOff()
	{
		_collider.enabled = false;
	}

	/// <summary>
	/// Включает коллайдер.
	/// </summary>
	public void SetPolygonColliderTurnOn()
	{
		_collider.enabled = true;
	}

	/// <summary>
	/// Отнимает у врага здоровье.
	/// </summary>
	/// <param name="damage">Получаемый урон.</param>
	public void TakeDamage(int damage)
	{
		_currentHealth -= damage;

		DetectDeath();
	}

	/// <summary>
	/// Отслеживает, когда враг должен умереть.
	/// </summary>
	private void DetectDeath()
	{
		if (_currentHealth <= 0)
		{
			//Die();
			Destroy(gameObject);
		}
	}

}
