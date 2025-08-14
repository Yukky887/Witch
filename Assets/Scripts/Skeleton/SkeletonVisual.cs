using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkeletonVisual : MonoBehaviour
{
	/// <summary>
	/// Подулючает компонент Animator для управления анимациями скелета.
	/// </summary>
	private Animator _animator;

	[SerializeField] private EnemyAI _enemyAI;

	/// <summary>
	/// Находится ли скелет в состоянии бега.
	/// </summary>
	private const string IS_RUNNING = "isRunning";

	/// <summary>
	/// Находится ли скелет в состоянии приследования.
	/// </summary>
	private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		_animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
		_animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
	}
}
