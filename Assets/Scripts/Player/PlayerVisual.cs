using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private FlashBlink _flashBlink;

	/// <summary>
	/// �������� ��� ���������� ���������� ������.
	/// </summary>
	private Animator _animator;

	/// <summary>
	/// �������� ������� ��� ���������� ���������� ������������ ������.
	/// </summary>
	private SpriteRenderer spriteRenderer;

	/// <summary>
	/// ��������� ��� ����������� ��������� ���� ������.
	/// </summary>
	private const string isRunning = "IsRunning";

	private const string IS_DIE = "isDie";

	/// <summary>
	/// ������������� ����������� ������ ��� �������.
	/// </summary>
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

	/// <summary>
	/// ��������� ���������� ������� ������ ������ ����.
	/// </summary>
	private void Update()
    {
		_animator.SetBool(isRunning, Player.Instance.IsRunning());
		
		if (Player.Instance.IsAlive())
		{
			AdjustPlayerFacingDirection(); 
		}

    }

	/// <summary>
	/// ������������ ������ � ������� ����.
	/// </summary>
	private void AdjustPlayerFacingDirection()
    {
        Vector3 mosePos = GameInput.Instance.GetMousePosition();
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
}
