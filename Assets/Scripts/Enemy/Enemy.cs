using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PxlDev
{
	public class Enemy : MonoBehaviour
	{
		[Header("Movement")]
		[SerializeField] protected Transform _player;
		[SerializeField] protected float _speed;
		[SerializeField] protected float _gravity;
		protected CharacterController _controller;

		[Space(15)]
		[SerializeField] protected float _knockbackAmount;
		[SerializeField] protected float _knockbackTime;


		[Header("AI")]
		[SerializeField] protected float _maxFollowDistance;
		[SerializeField] protected float _minFollowDistance;
		[SerializeField] protected Vector3 _movementDirection;

		[Header("States")]
		[SerializeField] protected bool _canMove;
		[SerializeField] protected bool _knocked;


		private AnimationController _anim;

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_anim = GetComponent<AnimationController>();
		}

		private void Update()
		{
			var InRange = Vector3.Distance(transform.position, _player.position) > _minFollowDistance && Vector3.Distance(transform.position, _player.position) < _maxFollowDistance;
			
			_anim.Still = _controller.velocity.x == 0 && _controller.velocity.z == 0;

			if(_movementDirection.x != 0 || _movementDirection.z != 0)
				_anim.SetDirection(new Vector2(_movementDirection.x, _movementDirection.z));

			if(_knocked)
			{
				Knocked();
				return;
			}

			if(!_canMove || !InRange)
				return;

			MovementCalculate();
			_controller.Move(_movementDirection * _speed * Time.deltaTime);
		}

		private void MovementCalculate()
		{
			_movementDirection = _player.position - transform.position;
			_movementDirection.y = 0;

			_movementDirection.Normalize();

			if(Mathf.Abs(_movementDirection.x) < 0.1f)
				_movementDirection.x = 0;

			if(Mathf.Abs(_movementDirection.z) < 0.1f)
				_movementDirection.z = 0;

			if(_movementDirection.x != 0)
			{
				if(_movementDirection.x > 0)
					_movementDirection.x = 1;

				if(_movementDirection.x < 0)
					_movementDirection.x = -1;
			}

			if(_movementDirection.z != 0)
			{
				if(_movementDirection.z > 0)
					_movementDirection.z = 1;

				if(_movementDirection.z < 0)
					_movementDirection.z = -1;
			}

			if(!_controller.isGrounded)
            {
                _movementDirection.y -= _gravity;
			}
		}

		private void Knocked()
		{
			float timer = (_knockbackTime / 2f) - Time.deltaTime;
			if(timer < 0)
				timer /= 2;

			_controller.Move(_movementDirection * _knockbackAmount * _knockbackTime);
		}

		public void Knockback(Transform knocker)
		{
			_knocked = true;
			_movementDirection = transform.position - knocker.position;
			_movementDirection.y = 0;
			StartCoroutine(KnockTimer());
		}

		private IEnumerator KnockTimer()
		{
			yield return new WaitForSeconds(_knockbackTime);
			_knocked = false;
			_canMove = false;

			yield return new WaitForSeconds(_knockbackTime * 1.5f);
			_canMove = true;
		}
	}
}