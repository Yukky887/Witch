using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
	/// <summary>
	/// Скорость движения игрока.	
	/// </summary>
	[SerializeField] private float movingSpeed = 10f;

	/// <summary>
	/// Управляемый Rigidbody2D игрока.
	/// </summary>
	private Rigidbody2D rb;

	/// <summary>
	/// Минимальная скорость движения, ниже которой игрок не считается движущимся.
	/// </summary>
	private float minMovingSpeed = 0.1f;

	/// <summary>
	/// Движется ли игрок в данный момент.
	/// </summary>
	private bool isRunning = false;

	private int comboStep = 0;

	/// <summary>
	/// Вектор ввода игрока.
	/// </summary>
	Vector2 inputVector;

	/// <summary>
	/// Игрок.
	/// </summary>
	public static Player Instance { get; private set; }

	/// <summary>
	/// Инициализация игрока.
	/// </summary>
	private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

	/// <summary>
	/// Подписывается на события ввода игрока при старте игры.
	/// </summary>
	private void Start()
	{
		GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
	}

	/// <summary>
	/// Обработчик события атаки игрока.
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
	/// Вызывается каждый кадр для обновления состояния игрока.
	/// </summary>
	private void Update()
    {
        inputVector = GameInput.Instance.GetMovemaentVector();
    }

	/// <summary>
	/// Движение игрока с фиксированной частотой кадров.
	/// </summary>
	private void FixedUpdate()
    {
        HandleMovement();
    }

	/// <summary>
	/// Обрабатывает движение игрока на основе ввода.
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
	/// Движется ли игрок в данный момент.
	/// </summary>
	/// <returns>True если игрок движется, False если не движется.</returns>
	public bool IsRunning()
    {
        return isRunning;
    }

	/// <summary>
	/// Получает позицию игрока на экране.
	/// </summary>
	/// <returns>Позиция игрока на экране.</returns>
	public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
