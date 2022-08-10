using System.Collections;
using TMPro;
using UnityEngine;

namespace TeamMelon.Utils
{
    public class TextAnim : MonoBehaviour
    {
        public bool Stop;
        
        [SerializeField] private float _delay;
        [SerializeField] private string[] _texts;

        private TextMeshProUGUI _textObj;

        private void Awake() => _textObj = GetComponent<TextMeshProUGUI>();

        private IEnumerator Start()
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                if (Stop) yield break;
                _textObj.text = _texts[i];
                yield return new WaitForSeconds(_delay);
            }

            StartCoroutine(Start());
        }
    }
}
