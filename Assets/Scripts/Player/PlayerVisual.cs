using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	/// <summary>
	/// �������� ��� ���������� ���������� ������.
	/// </summary>
	private Animator animator;

	/// <summary>
	/// �������� ������� ��� ���������� ���������� ������������ ������.
	/// </summary>
	private SpriteRenderer spriteRenderer;

	/// <summary>
	/// ��������� ��� ����������� ��������� ���� ������.
	/// </summary>
	private const string isRunning = "IsRunning";


	/// <summary>
	/// ������������� ����������� ������ ��� �������.
	/// </summary>
	private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

	/// <summary>
	/// ��������� ���������� ������� ������ ������ ����.
	/// </summary>
	private void Update()
    {
        animator.SetBool(isRunning, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection(); 
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
