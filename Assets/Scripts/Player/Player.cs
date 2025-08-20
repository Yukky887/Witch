using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
	/// <summary>
	/// �������� �������� ������.	
	/// </summary>
	[SerializeField] private float movingSpeed = 10f;

	/// <summary>
	/// ����������� Rigidbody2D ������.
	/// </summary>
	private Rigidbody2D rb;

	/// <summary>
	/// ����������� �������� ��������, ���� ������� ����� �� ��������� ����������.
	/// </summary>
	private float minMovingSpeed = 0.1f;

	/// <summary>
	/// �������� �� ����� � ������ ������.
	/// </summary>
	private bool isRunning = false;

	private int comboStep = 0;

	/// <summary>
	/// ������ ����� ������.
	/// </summary>
	Vector2 inputVector;

	/// <summary>
	/// �����.
	/// </summary>
	public static Player Instance { get; private set; }

	/// <summary>
	/// ������������� ������.
	/// </summary>
	private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

	/// <summary>
	/// ������������� �� ������� ����� ������ ��� ������ ����.
	/// </summary>
	private void Start()
	{
		GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
	}

	/// <summary>
	/// ���������� ������� ����� ������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void GameInput_OnPlayerAttack(Sword.SwordAttackType _)
	{
		Sword.SwordAttackType typeToUse;

		if (comboStep == 0)
		{
			typeToUse = Sword.SwordAttackType.Light;
			comboStep = 1;
		}
		else
		{
			typeToUse = Sword.SwordAttackType.Strong;
			comboStep = 0;
		}

		ActiveWeapon.Instance.GetActiveWeapon().Attack(typeToUse);
	}

	/// <summary>
	/// ���������� ������ ���� ��� ���������� ��������� ������.
	/// </summary>
	private void Update()
    {
        inputVector = GameInput.Instance.GetMovemaentVector();
    }

	/// <summary>
	/// �������� ������ � ������������� �������� ������.
	/// </summary>
	private void FixedUpdate()
    {
        HandleMovement();
    }

	/// <summary>
	/// ������������ �������� ������ �� ������ �����.
	/// </summary>
	private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovemaentVector();

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

	/// <summary>
	/// �������� �� ����� � ������ ������.
	/// </summary>
	/// <returns>True ���� ����� ��������, False ���� �� ��������.</returns>
	public bool IsRunning()
    {
        return isRunning;
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
}
