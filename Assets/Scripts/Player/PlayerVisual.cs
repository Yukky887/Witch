using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	private static readonly int Die = Animator.StringToHash(IsDie);
	private static readonly int Running = Animator.StringToHash(IsRunning);
	
	private Player _player;
	private FlashBlink _flashBlink;
	
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;
	
	private const string IsRunning = "IsRunning";
	private const string IsDie = "isDie";
	
	private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
		_flashBlink = GetComponent<FlashBlink>();
	}

	private void Start()
	{
		Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
	}

	private void Player_OnPlayerDeath()
	{
		_animator.SetBool(Die, true);
		_flashBlink.StopBlinking();
		_spriteRenderer.sortingOrder = -1;
	}
	
	private void Update()
    {
		_animator.SetBool(Running, Player.Instance.IsRunning());
		
		if (Player.Instance.IsAlive())
		{
			AdjustPlayerFacingDirection(); 
		}
    }
	
	private void AdjustPlayerFacingDirection()
    {
        if (Player.Instance.IsAttacking)
        {
	        return;
        }
        
        var mosePos = GameInput.GetMousePosition();
        var playerPosition = Player.Instance.GetPlayerScreenPosition();
		
        _spriteRenderer.flipX = mosePos.x < playerPosition.x;
    }
	
	private void OnDestroy()
    { 
	    Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
