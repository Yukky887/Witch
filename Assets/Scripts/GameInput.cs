using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    /// <summary>
    /// Нажатие на кнопку аттаки. 
    /// </summary>
    public event Action<Sword.SwordAttackType> OnPlayerAttack;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
		playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }

	/// <summary>
	/// Подписывается на событие начала атаки игрока.
	/// </summary>
	private void PlayerAttack_started(InputAction.CallbackContext context)
	{
		OnPlayerAttack?.Invoke(Sword.SwordAttackType.Light);
	}

	/// <summary>
	/// Получает вектор движения.
	/// </summary>
	/// <returns>Вектор движения.</returns>
	public Vector2 GetMovemaentVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    /// <summary>
    /// Получает позицию мышы для разварота ГГ.
    /// </summary>
    /// <returns>Позиция мышы.</returns>
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        return mousePos;
    }

}
