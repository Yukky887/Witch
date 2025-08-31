using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{	
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;
    
    public event Action<Sword.SwordAttackType> OnPlayerAttack;

    public event Action OnPlayerDash;

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
		_playerInputActions.Combat.Attack.started += PlayerAttack_started;
		_playerInputActions.Player.Dash.started += PlayerDash_performed;
    }

	public Vector2 GetMovementVector()
    {
        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
	
    public static Vector3 GetMousePosition()
    {
        var mousePos = Mouse.current.position.ReadValue();

        return mousePos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
	}
	
    private void PlayerDash_performed(InputAction.CallbackContext obj)
    {
	    OnPlayerDash?.Invoke();
    }
    
	private void PlayerAttack_started(InputAction.CallbackContext context)
	{
		OnPlayerAttack?.Invoke(Sword.SwordAttackType.Light);
	}
	
    private void OnDestroy()
    {
	    _playerInputActions.Combat.Attack.started -= PlayerAttack_started;
	    _playerInputActions.Player.Dash.started -= PlayerDash_performed;
    }
}
