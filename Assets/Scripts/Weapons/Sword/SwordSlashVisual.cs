using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
	/// <summary>
	/// Тип атаки меча.
	/// </summary>
	[SerializeField] private Sword.SwordAttackType swordAttackType;

	/// <summary>
	/// Привязка меча к визуальным эффектам удара.
	/// </summary>
	[SerializeField] private Sword sword;

	/// <summary>
	/// Триггер для запуска анимации удара меча.
	/// </summary>
	const string attack = "Attack";

	/// <summary>
	/// Инициализация аниматора для управления анимациями удара меча.
	/// </summary>
	private Animator animator;

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
		sword.OnSwordSwing += OnSwordSwing;
	}

	/// <summary>
	/// Выполняет анимацию удара меча при срабатывании события OnSwordSwing.
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
	/// Отписывается от события удара меча.
	/// </summary>
	private void OnDestroy()
	{
		sword.OnSwordSwing -= OnSwordSwing;
	}
}
