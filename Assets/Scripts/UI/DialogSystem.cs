using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PxlDev
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField] private Queue<string> _sentences;
        [SerializeField] private CanvasGroup _dialogueBox;

        [Space(15)]
        [SerializeField] private float _typeSpeed;
        private float _curTypeSpeed;
        [SerializeField] private TextMeshProUGUI _dialog;
        [SerializeField] private TextMeshProUGUI _name;

        [Space(15)]
        [SerializeField] private string _pauseLetters = ",.?-";
        [SerializeField] private float _pauseTime;

        public static DialogSystem Instance;

        private bool _isTyping;
        private bool _hasDialogue;

        void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        void Start()
        {
            _sentences = new Queue<string>();
        }

        void Update()
        {
            if(Input.GetButton("Jump") && _isTyping)
            {
                _curTypeSpeed = _typeSpeed / 5f;
            }
            else if(_isTyping)
            {
                _curTypeSpeed = _typeSpeed;
            }

            if(Input.GetButtonDown("Jump") && !_isTyping && _hasDialogue)
            {
                NextSentence();
            }


        }

        private UnityEvent _onFinish;
        public void StartDialogue(Dialog dialog, UnityEvent onFinish)
        {
            _onFinish = onFinish;
            _dialogueBox.DOFade(1, 0.25f);

            _sentences.Clear();
            _name.text = dialog.Name;
            
            _hasDialogue = true;

            foreach(string sentence in dialog.Lines)
            {
                _sentences.Enqueue(sentence);
            }

            NextSentence();
        }

        void NextSentence()
        {
            if(_sentences.Count == 0)
            {
                EndDialogue(_onFinish);
                _onFinish = null;
                _hasDialogue = false;
                return;
            }

            string sentence = _sentences.Dequeue();

            StopAllCoroutines();

            _isTyping = true;
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            _dialog.text = "";
            foreach(char letter in sentence.ToCharArray())
            {
                _dialog.text += letter;

                if(_pauseLetters.Contains(letter.ToString()))
                    yield return new WaitForSeconds(_pauseTime);
                else
                    yield return new WaitForSeconds(_curTypeSpeed);
            }
            _isTyping = false;
        }

        void EndDialogue(UnityEvent onFinish)
        {
            StopAllCoroutines();

            _dialogueBox.DOFade(0, 0.25f);
            onFinish?.Invoke();

            StartCoroutine(InteractionBackTimer());
        }

        IEnumerator InteractionBackTimer()
        {

            yield return new WaitForSeconds(0.4f);
            Player.Instance.Interactions.IsInteracting = false;
        }
    }
}