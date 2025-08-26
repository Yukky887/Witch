using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{

	[SerializeField] private Sword.SwordAttackType swordAttackType;
	[SerializeField] private Sword sword;
	
	const string attack = "Attack";
	private Animator animator;
	
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
	private void Start()
	{
		sword.OnSwordSwing += OnSwordSwing;
	}
	
	private void OnSwordSwing(Sword.SwordAttackType attackType)
	{
		if (attackType != swordAttackType)
		{
			return;
		}

		animator.SetTrigger(attack);
	}
	
	private void OnDestroy()
	{
		sword.OnSwordSwing -= OnSwordSwing;
	}
}
