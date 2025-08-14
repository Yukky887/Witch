using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
	/// <summary>
	/// ��� ����� ����.
	/// </summary>
	[SerializeField] private Sword.SwordAttackType swordAttackType;

	/// <summary>
	/// �������� ���� � ���������� �������� �����.
	/// </summary>
	[SerializeField] private Sword sword;

	/// <summary>
	/// ������� ��� ������� �������� ����� ����.
	/// </summary>
	const string attack = "Attack";

	/// <summary>
	/// ������������� ��������� ��� ���������� ���������� ����� ����.
	/// </summary>
	private Animator animator;

	/// <summary>
	/// ��������� �������� ��� ������������� �������.
	/// </summary>
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	/// <summary>
	/// ������������� �� ������� ����� ���� ��� ������� �������.
	/// </summary>
	private void Start()
	{
		sword.OnSwordSwing += OnSwordSwing;
	}

	/// <summary>
	/// ��������� �������� ����� ���� ��� ������������ ������� OnSwordSwing.
	/// </summary>
	private void OnSwordSwing(Sword.SwordAttackType attackType)
	{
		if (attackType != swordAttackType)
		{
			return;
		}

		animator.SetTrigger(attack);
	}

	/// <summary>
	/// ������������ �� ������� ����� ����.
	/// </summary>
	private void OnDestroy()
	{
		sword.OnSwordSwing -= OnSwordSwing;
	}
}
