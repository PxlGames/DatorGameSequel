using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PxlDev
{
	public class Interactable : MonoBehaviour
	{
		public UnityEvent _event;

		public void Interact()
		{
			_event?.Invoke();
		}
	}
}