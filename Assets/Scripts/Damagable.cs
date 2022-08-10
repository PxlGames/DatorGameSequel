using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PxlDev
{
	public class Damageable : MonoBehaviour
	{
		public int Health;
		protected bool _hasDied;

		void Update()
		{
			if(Health <= 0 && !_hasDied)
			{
				Death();
			}
		}

		public virtual void Damage(int damage, Transform attackObject)
		{
			Health -= damage;
		}

		public virtual void Death()
		{
			_hasDied = true;
			Destroy(gameObject);
		}
	}
}