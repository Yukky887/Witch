using UnityEngine;

/// <summary>
/// Отброс игрока.
/// </summary>
public class KnockBack : MonoBehaviour
{

	/// <summary>
	/// Сила отброса.
	/// </summary>
	[SerializeField] private float _knockBackForce = 3f;

	/// <summary>
	/// Максимальное время отброса.
	/// </summary>
	[SerializeField] private float _knockBackMovingTimerMax = 0.3f;


	/// <summary>
	/// Таймер отброса.
	/// </summary>
	private float _knockBackMovingTimer;

	/// <summary>
	/// Rigidbody2D игрока.
	/// </summary>
	private Rigidbody2D _rb;

	public bool IsTakingKnockBack { get; private set; }

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		_knockBackMovingTimer -= Time.deltaTime;

		if ( _knockBackMovingTimer < 0 )
		{
			StopKnockBackMovment();
		}
	}

	/// <summary>
	/// Расчитывает и применяет отброс игрока.
	/// </summary>
	/// <param name="damageSource">Источник отброса.</param>
	public void GetKnockBack(Transform damageSource)
	{
		IsTakingKnockBack = true;
		_knockBackMovingTimer = _knockBackMovingTimerMax;
		Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / _rb.mass;
		_rb.AddForce(difference, ForceMode2D.Impulse);
	}

	/// <summary>
	/// Останавливает отброс игрока.
	/// </summary>
	public void StopKnockBackMovment()
	{
		_rb.velocity = Vector2.zero;

		IsTakingKnockBack = false;
	}
}
