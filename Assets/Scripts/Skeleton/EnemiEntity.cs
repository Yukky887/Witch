using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemiEntity : MonoBehaviour
{

	[SerializeField] private Enemy enemy;

	public event Action OnTakeHit;

	public event Action OnDie;

	private int _currentHealth;

	private PolygonCollider2D _collider;

	private BoxCollider2D _boxCollider;

	private EnemyAI _enemyAI;

	private void Awake()
	{
		_collider = GetComponent<PolygonCollider2D>();
		_boxCollider = GetComponent<BoxCollider2D>();
		_enemyAI = GetComponent<EnemyAI>();
	}

	private void Start()
	{
		_currentHealth = enemy.enemyHealth;
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.transform.TryGetComponent(out Player player))
		{
			player.TakeDamage(transform, enemy.enemyDamageAmount);
		}
		
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log($"Collision with {collision.gameObject.name} detected.");
	}
	
	public void SetPolygonColliderTurnOff()
	{
		_collider.enabled = false;
	}
	
	public void SetPolygonColliderTurnOn()
	{
		_collider.enabled = true;
	}
	
	public void TakeDamage(int damage)
	{
		_currentHealth -= damage;

		OnTakeHit?.Invoke();

		DetectDeath();
	}

	private void DetectDeath()
	{
		if (_currentHealth > 0)
		{
			return;
		}
		
		_boxCollider.enabled = false;
		_collider.enabled = false;
		_enemyAI.SetDeadState();
		OnDie?.Invoke();
	}

}
