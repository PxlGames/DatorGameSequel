using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PxlDev
{
	public class EntityShadow : MonoBehaviour
	{
		[SerializeField] private float _maxCheckDistance;
		[SerializeField] private Transform _shadow;

		private void FixedUpdate()
		{
			if (Physics.Raycast(transform.position, -Vector3.up, out var hit))
			{	
				_shadow.gameObject.SetActive(true);
				_shadow.position = hit.point + new Vector3(0, 0.001f, 0);
				_shadow.rotation = Quaternion.LookRotation(hit.normal);
			}
			else _shadow.gameObject.SetActive(false);
		}
	}
}