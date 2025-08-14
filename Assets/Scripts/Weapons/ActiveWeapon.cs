using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// ���������� �������� ������ ������ � ����.
/// </summary>
public class ActiveWeapon : MonoBehaviour
{

	public static ActiveWeapon Instance { get; private set; }

	private void Update()
	{
		FillowMausPosition();
	}

	/// <summary>
	/// ������ �� �������� ������, ������� ����� �������������� � ����.
	/// </summary>
	[SerializeField] private Sword sword;

	private void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// ���������� ������� �������� ������ ������.
	/// </summary>
	/// <returns>������� �������� ������ ������.</returns>
	public Sword GetActiveWeapon()
	{
		return sword;
	}

	/// <summary>
	/// ������������ ������ � ������� ����.
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
