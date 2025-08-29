using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
	[SerializeField] private Sword sword;
	
	public static ActiveWeapon Instance { get; private set; }

	private void Update()
	{
		if (Player.Instance.IsAlive())
		{
			FollowMousePosition();
		}
	}
	
	private void Awake()
	{
		Instance = this;
	}

	public Sword GetActiveWeapon()
	{
		return sword;
	}
	
	private void FollowMousePosition()
	{
		var mosePos = GameInput.GetMousePosition();
		var playerPosition = Player.Instance.GetPlayerScreenPosition();

		transform.rotation = Quaternion.Euler(0, mosePos.x < playerPosition.x ? 180 : 0, 0);
	}
}
