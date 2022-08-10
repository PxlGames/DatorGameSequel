using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PxlDev
{
	public class AnimationController : MonoBehaviour
	{
		public Vector2 Direction;

		[Space(15)]
		public bool Still;
		public bool IsDead;
		// public bool Mine;s

		private bool hasDied;

		[HideInInspector] public Animator _anim;

		void Start()
		{
			_anim = GetComponent<Animator>();
		}

		public void SetDirection(Vector2 direction)
		{
			if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				direction.y = 0;
			} 
			else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
			{
				direction.x = 0;
			}
			
			Direction = direction;
		}

		void Update()
		{
			_anim.SetFloat("x", Direction.x);
			_anim.SetFloat("y", Direction.y);

			_anim.SetBool("Still", Still);

			if(!hasDied && IsDead)
			{
				hasDied = true;
				_anim.SetTrigger("Death");
			}

			// _anim.SetBool("Miner", Mine);
		}

		public void Damaged()
        {
            _anim.SetTrigger("Hit");
			print("hit");
        }
	}
}