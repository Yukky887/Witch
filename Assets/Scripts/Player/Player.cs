using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
	
	[SerializeField] private float movingSpeed = 10f;
	[SerializeField] private int maxHealth = 30;
	[SerializeField] private float damageRecoveryTime = 0.5f;

	
	private Rigidbody2D _rb;
	private KnockBack _knockBack;
	private int _currentHealth;
	private bool _isAlive;
	private const float MinMovingSpeed = 0.1f;
	private bool _isRunning;
	private bool _canTakeDamage = true;
	private int _comboStep;
	
	private Camera _camera;
	
	private Vector2 _inputVector;
	
	public static Player Instance { get; private set; }

	public event Action TakeHit;
	public event Action OnPlayerDeath;
	
	private void Awake()
    {
	    _camera = Camera.main;
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
		_knockBack = GetComponent<KnockBack>();
	}
	
	private void Start()
	{
		_isAlive = true;
		_currentHealth = maxHealth;
		GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
	}
	
	private void Update()
    {
	    _inputVector = GameInput.Instance.GetMovementVector();
    }
	
	public bool IsRunning()
    {
        return _isRunning;
    }

	public bool IsAlive()
	{
		return _isAlive;
	}
	
	public Vector3 GetPlayerScreenPosition()
    {
        var playerScreenPosition = _camera.WorldToScreenPoint(transform.position);
        
        return playerScreenPosition;
    }
	
	public void TakeDamage(Transform damageSource, int damage)
	{
		if (_canTakeDamage && _isAlive)
		{
			TakeHit?.Invoke();
			_canTakeDamage = false;
			_currentHealth = Mathf.Max(0, _currentHealth -= damage);
			Debug.Log(_currentHealth);
			_knockBack.GetKnockBack(damageSource);
			StartCoroutine(DamageRecoveryRouting());
		}

		DetectDeath();
	}

	private void DetectDeath()
	{
		if (_currentHealth > 0 || !_isAlive)
		{
			return;
		}
		_isAlive = false;
		GameInput.Instance.DisableMovement();
		_knockBack.StopKnockBackMovement();
		OnPlayerDeath?.Invoke();
	}
	
	private IEnumerator DamageRecoveryRouting()
	{
		yield return new WaitForSeconds(damageRecoveryTime);
		_canTakeDamage = true;
	} 
	
	private void GameInput_OnPlayerAttack(Sword.SwordAttackType _)
	{
		Sword.SwordAttackType typeToUse;

		if (_comboStep == 0)
		{
			typeToUse = Sword.SwordAttackType.Light;
			_comboStep = 1;
		}
		else
		{
			typeToUse = Sword.SwordAttackType.Strong;
			_comboStep = 0;
		}

		ActiveWeapon.Instance.GetActiveWeapon().Attack(typeToUse);
	}
	
	private void FixedUpdate()
    {
		if (_knockBack.IsTakingKnockBack)
		{
			return;
		}

		HandleMovement();
    }
	
	private void HandleMovement()
    {
        var vector2 = GameInput.Instance.GetMovementVector();

        _rb.MovePosition(_rb.position + vector2 * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(vector2.x) > MinMovingSpeed || Mathf.Abs(vector2.y) > MinMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

	private void OnDestroy()
	{
		GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
	}
}
