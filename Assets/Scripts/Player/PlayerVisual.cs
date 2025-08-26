using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private FlashBlink _flashBlink;
	
	private Animator _animator;
	private SpriteRenderer spriteRenderer;
	
	private const string isRunning = "IsRunning";
	private const string IS_DIE = "isDie";
	
	private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
		_flashBlink = GetComponent<FlashBlink>();
	}

	private void Start()
	{
		Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
	}

	private void Player_OnPlayerDeath()
	{
		_animator.SetBool(IS_DIE, true);
		_flashBlink.StopBlinking();
	}
	
	private void Update()
    {
		_animator.SetBool(isRunning, Player.Instance.IsRunning());
		
		if (Player.Instance.IsAlive())
		{
			AdjustPlayerFacingDirection(); 
		}

    }
	
	private void AdjustPlayerFacingDirection()
    {
        Vector3 mosePos = GameInput.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mosePos.x < playerPosition.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
	
	private void OnDestroy()
    { 
	    Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
