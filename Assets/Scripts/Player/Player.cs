using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
	public event Action OnPlayerDeath;

	/// <summary>
	/// �������� �������� ������.	
	/// </summary>
	[SerializeField] private float _movingSpeed = 10f;

	[SerializeField] private int _maxHealth = 30;

	[SerializeField] private float _damageRecoveryTime = 0.5f;


	/// <summary>
	/// ����������� Rigidbody2D ������.
	/// </summary>
	private Rigidbody2D rb;

	private KnockBack _knockBack;

	private int _currentHealth;

	private bool _isAlive;

	/// <summary>
	/// ����������� �������� ��������, ���� ������� ����� �� ��������� ����������.
	/// </summary>
	private float _minMovingSpeed = 0.1f;

	/// <summary>
	/// �������� �� ����� � ������ ������.
	/// </summary>
	private bool _isRunning = false;

	private bool _canTakeDamage = true;

	private int _comboStep = 0;

	/// <summary>
	/// ������ ����� ������.
	/// </summary>
	Vector2 inputVector;

	/// <summary>
	/// �����.
	/// </summary>
	public static Player Instance { get; private set; }

	public event Action TakeHit;

	/// <summary>
	/// ������������� ������.
	/// </summary>
	private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
		_knockBack = GetComponent<KnockBack>();
	}

	/// <summary>
	/// ������������� �� ������� ����� ������ ��� ������ ����.
	/// </summary>
	private void Start()
	{
		_isAlive = true;
		_currentHealth = _maxHealth;
		GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
	}


	/// <summary>
	/// ���������� ������ ���� ��� ���������� ��������� ������.
	/// </summary>
	private void Update()
    {
        inputVector = GameInput.Instance.GetMovemaentVector();
    }

	/// <summary>
	/// �������� �� ����� � ������ ������.
	/// </summary>
	/// <returns>True ���� ����� ��������, False ���� �� ��������.</returns>
	public bool IsRunning()
    {
        return _isRunning;
    }

	public bool IsAlive()
	{
		return _isAlive;
	}

	/// <summary>
	/// �������� ������� ������ �� ������.
	/// </summary>
	/// <returns>������� ������ �� ������.</returns>
	public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

	/// <summary>
	/// ��������� ���� � ������ � �������� ������ �������.
	/// </summary>
	/// <param name="damageSource">��������� ���������� ����</param>
	/// <param name="damage">����</param>
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
		if (_currentHealth <= 0 && _isAlive)
		{
			_isAlive = false;
			GameInput.Instance.DisableMovment();
			_knockBack.StopKnockBackMovment();
			OnPlayerDeath?.Invoke();
		}
	}

	/// <summary>
	/// �������� �������������� ����������� ��������� ����� �������.
	/// </summary>
	/// <returns></returns>
	private IEnumerator DamageRecoveryRouting()
	{
		yield return new WaitForSeconds(_damageRecoveryTime);
		_canTakeDamage = true;
	} 

	/// <summary>
	/// ���������� ������� ����� ������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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

	/// <summary>
	/// �������� ������ � ������������� �������� ������.
	/// </summary>
	private void FixedUpdate()
    {
		if (_knockBack.IsTakingKnockBack)
		{
			return;
		}

		HandleMovement();
    }

	/// <summary>
	/// ������������ �������� ������ �� ������ �����.
	/// </summary>
	private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovemaentVector();

        rb.MovePosition(rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

}
