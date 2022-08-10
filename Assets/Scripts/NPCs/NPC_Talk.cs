using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PxlDev
{
	public class NPC_Talk : MonoBehaviour
	{
        [SerializeField] private Dialog _dialog;
        [SerializeField] private UnityEvent _onDialogFinish;

        public virtual void StartDialog()
        {
            DialogSystem.Instance.StartDialogue(_dialog, _onDialogFinish);
            // SFXManager.Play("NPC_Interaction");
        }
	}
}