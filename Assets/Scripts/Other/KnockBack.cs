using UnityEngine;

public class KnockBack : MonoBehaviour
{
	[SerializeField] private float knockBackForce = 3f;
	[SerializeField] private float knockBackMovingTimerMax = 0.3f;

	private float _knockBackMovingTimer;
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
			StopKnockBackMovement();
		}
	}
	
	public void GetKnockBack(Transform damageSource)
	{
		IsTakingKnockBack = true;
		_knockBackMovingTimer = knockBackMovingTimerMax;
		Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce / _rb.mass;
		_rb.AddForce(difference, ForceMode2D.Impulse);
	}
	
	public void StopKnockBackMovement()
	{
		_rb.velocity = Vector2.zero;

		IsTakingKnockBack = false;
	}
}
