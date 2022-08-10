using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PxlDev
{
	public class NPC_Walking : MonoBehaviour
	{
		[Header("Positions")]
		public bool move;
		public float speed;
		[Space(15)]
		public float[] waitingTimes;
		public Transform[] _points;
		public Vector3[] _positions;
		[Space(15)]
		public int pos;
		public bool Positioned;
		public bool canMove;

		[Header("Animation Variables")]
		[SerializeField] private AnimationController _npc_anim;

		void Start()
		{
			_positions = new Vector3[_points.Length];
			for (int i = 0; i < _points.Length; i++)
			{
				_positions[i] = _points[i].position;
			}

			foreach(var pos in _points)
			{
				Destroy(pos.gameObject);
			}
		}

		private Vector3 _lastPosition;
		private bool IsStill()
		{
			bool still;

			if(_lastPosition == transform.position)
				still = true;
			else
				still = false;

			_lastPosition = transform.position;

			return still;
		}

		void OnValidate()
		{
			if(_points.Length != waitingTimes.Length)
			{
				waitingTimes = new float[_points.Length];
			}
		}

		void Update()
		{	
			_npc_anim.Still = IsStill();
			if(_positions.Length != pos && move && canMove)
			{
				var direction = ((transform.position - _positions[pos])).normalized;
				_npc_anim.SetDirection(new Vector2(direction.x, direction.z));

				transform.position = Vector3.MoveTowards(transform.position, _positions[pos], speed * Time.deltaTime);
				if(transform.position == _positions[pos])
				{
					StartCoroutine(waitTimer());
				}

			} else if(canMove)
			{
				pos = 0;
			}

			if(pos > _positions.Length)
			{
				pos = 0;
			}
		}

		IEnumerator waitTimer()
		{
			canMove = false;
			yield return new WaitForSeconds(waitingTimes[pos]);
			canMove = true;
			pos++;
		}
		
		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
            if (UnityEditor.EditorApplication.isPlaying)
                  return;

			Gizmos.color = Color.blue;
			for (int i = 0; i < _points.Length; i++)
			{
				if(i == _points.Length - 1)
				{
					Gizmos.DrawLine(_points[i].position, _points[0].position);
				} 
				else
				{
					Gizmos.DrawLine(_points[i].position, _points[i+1].position);
				}

				Gizmos.DrawWireSphere(_points[i].position, 0.5f);
			}
		}
		#endif
	}
}