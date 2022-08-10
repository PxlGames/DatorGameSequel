using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PxlDev
{
	public class EnemyHP : Damageable
	{
		private Enemy _enemy;

		protected AnimationController _anim;

		void Start()
		{
			_enemy = GetComponent<Enemy>();
			TryGetComponent<AnimationController>(out _anim);
		}

		public override void Damage(int damage, Transform attackObject)
		{
			Health -= damage;
			
			if(attackObject != null)
				_enemy.Knockback(attackObject);
			
			if(_anim == null)
				return;

			_anim.Damaged();
		}

		public override void Death()
		{
			_hasDied = true;
			Destroy(gameObject);
		}
	}
}