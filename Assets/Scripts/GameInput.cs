using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{	
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    
    public event Action<Sword.SwordAttackType> OnPlayerAttack;

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
		_playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }
    private void OnDestroy()
    {
	    _playerInputActions.Combat.Attack.started -= PlayerAttack_started;
    }
	
	private void PlayerAttack_started(InputAction.CallbackContext context)
	{
		OnPlayerAttack?.Invoke(Sword.SwordAttackType.Light);
	}
	
	public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
	
    public static Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        return mousePos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
	}
}
