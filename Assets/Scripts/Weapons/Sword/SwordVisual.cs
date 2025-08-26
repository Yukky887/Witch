using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
	[SerializeField] private Sword sword;

	/// <summary>
	/// ������������� ��������� ��� ���������� ���������� ����� ����.
	/// </summary>
	private Animator animator;

	/// <summary>
	/// ������� ��� ������� �������� ������� ����� ����.
	/// </summary>
	const string attackLight = "AttackLight";

	/// <summary>
	/// ������� ��� ������� �������� �������� ����� ����.
	/// </summary>
	const string attackSrtong = "AttackStrong";

	/// <summary>
	/// ����� �� ������������ ������ ����.
	/// </summary>
	private bool canQueueNextAttack = false;

	/// <summary>
	/// ������������ �� ������ ������ ����.
	/// </summary>
	private bool queueNextAttack = false;

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
		sword.OnSwordSwing += Sword_OnSwordSwing;
	}

	/// <summary>
	/// ��������� �������� ����� ���� ��� ������������ ������� OnSwordSwing.
	/// </summary>
	private void Sword_OnSwordSwing(Sword.SwordAttackType type)
	{
		if (type == Sword.SwordAttackType.Light)
		{
			animator.SetTrigger(attackLight);
		}
		else if (type == Sword.SwordAttackType.Strong)
		{
			animator.SetTrigger(attackSrtong);
		}
	}

	public void TriggerStartWindowCombo()
	{
		canQueueNextAttack = true;

		if (queueNextAttack is true)
		{
			PlayNextAttack();
			queueNextAttack = false;
		}
	}
	public void TriggerEndWindowCombo()
	{
		canQueueNextAttack = false;
	}

	public void OnAttackInput()
	{
		if (canQueueNextAttack)
		{
			PlayNextAttack();
		} 
		else
		{
			queueNextAttack = true;
		}
	}

	private void PlayNextAttack()
	{
		sword.Attack(Sword.SwordAttackType.Strong);
		canQueueNextAttack = false;
		queueNextAttack = false;
	}

	public void TriggerEndAttackAnimation()
	{
		sword.AttackPoligonColliderTurnOff();
	}

	private void OnDestroy()
	{
		sword.OnSwordSwing -= Sword_OnSwordSwing;
	}
}
