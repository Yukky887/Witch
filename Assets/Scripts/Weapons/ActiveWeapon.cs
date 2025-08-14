using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// ќпредел€ет активное оружие игрока в игре.
/// </summary>
public class ActiveWeapon : MonoBehaviour
{

	public static ActiveWeapon Instance { get; private set; }

	private void Update()
	{
		FillowMausPosition();
	}

	/// <summary>
	/// —сылка на активное оружие, которое будет использоватьс€ в игре.
	/// </summary>
	[SerializeField] private Sword sword;

	private void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// ¬озвращает текущее активное оружие игрока.
	/// </summary>
	/// <returns>“екущее активное оружие игрока.</returns>
	public Sword GetActiveWeapon()
	{
		return sword;
	}

	/// <summary>
	/// ѕоворачивает оружее в сторону мыши.
	/// </summary>
	private void FillowMausPosition()
	{
		Vector3 mosePos = GameInput.Instance.GetMousePosition();
		Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

		if (mosePos.x < playerPosition.x)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		else
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
