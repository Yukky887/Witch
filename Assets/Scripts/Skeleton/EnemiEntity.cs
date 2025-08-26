using System;
using UnityEngine;

/// <summary>
/// ��������� �� ������, ������� ������ ���� �������� � ���������� Unity.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemiEntity : MonoBehaviour
{

	[SerializeField] private Enemy _enemy;
	//[SerializeField] private int _maxHealth;

	public event Action OnTakeHit;

	public event Action OnDie;

	private int _currentHealth;

	private PolygonCollider2D _collider;

	private BoxCollider2D _boxCollaider;

	private EnemyAI _enemyAI;

	private void Awake()
	{
		_collider = GetComponent<PolygonCollider2D>();
		_boxCollaider = GetComponent<BoxCollider2D>();
		_enemyAI = GetComponent<EnemyAI>();
	}

	private void Start()
	{
		_currentHealth = _enemy.enemyHealth;
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.transform.TryGetComponent(out Player player))
		{
			player.TakeDamage(transform, _enemy.enemyDamageAmount);
		}
		
	}

	/// <summary>
	/// ������������ ����������� ���������� ���� ������� � �������.
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log($"Collision with {collision.gameObject.name} detected.");
	}

	/// <summary>
	/// ��������� ���������.
	/// </summary>
	public void SetPolygonColliderTurnOff()
	{
		_collider.enabled = false;
	}

	/// <summary>
	/// �������� ���������.
	/// </summary>
	public void SetPolygonColliderTurnOn()
	{
		_collider.enabled = true;
	}

	/// <summary>
	/// �������� � ����� ��������.
	/// </summary>
	/// <param name="damage">���������� ����.</param>
	public void TakeDamage(int damage)
	{
		_currentHealth -= damage;

		OnTakeHit?.Invoke();

		DetectDeath();
	}

	/// <summary>
	/// �����������, ����� ���� ������ �������.
	/// </summary>
	private void DetectDeath()
	{
		if (_currentHealth <= 0)
		{
			_boxCollaider.enabled = false;
			_collider.enabled = false;
			_enemyAI.SetDeadState();
			OnDie?.Invoke();
		}
	}

}
