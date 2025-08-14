using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
	/// <summary>
	/// Приязка меча к визуальным эффектам удара.
	/// </summary>
	[SerializeField] private Sword sword;

	/// <summary>
	/// Инициализация аниматора для управления анимациями удара меча.
	/// </summary>
	private Animator animator;

	/// <summary>
	/// Триггер для запуска анимации легкого удара меча.
	/// </summary>
	const string attackLight = "AttackLight";

	/// <summary>
	/// Триггер для запуска анимации сильного удара меча.
	/// </summary>
	const string attackSrtong = "AttackStrong";

	/// <summary>
	/// Можно ли использовать второй удар.
	/// </summary>
	private bool canQueueNextAttack = false;

	/// <summary>
	/// Используется ли сейчас второй удар.
	/// </summary>
	private bool queueNextAttack = false;

	/// <summary>
	/// Запускает аниматор при инициализации объекта.
	/// </summary>
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Подписывается на событие удара меча при запуске объекта.
	/// </summary>
	private void Start()
	{
		sword.OnSwordSwing += Sword_OnSwordSwing;
	}

	/// <summary>
	/// Выполняет анимацию удара меча при срабатывании события OnSwordSwing.
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
}
