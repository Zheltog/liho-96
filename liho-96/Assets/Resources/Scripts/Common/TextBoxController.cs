using System.Collections;
using TMPro;
using UnityEngine;

namespace Common
{
    public class TextBoxController : MonoBehaviour
    {
        public float defaultSecondsBeforeNextSymbol = 0.075f;
        public float secondsBeforeNextSymbol = 0.075f;
        public float delayMultiplier = 5;
        public float pitchRangeMin = 0.7f;
        public float pitchRangeMax = 0.8f;
        public bool IsPrinting { get; private set; }
        public AudioController audioController;

        private char[] _currentPhraseChars;
        private string _currentPhrase;
        private string _currentPhraseFinal;
        private TextMeshProUGUI _textBox;

        private void Start()
        {
            _textBox = GetComponent<TextMeshProUGUI>();
        }

        public void FinishPrinting()
        {
            _textBox.text = _currentPhraseFinal;
            IsPrinting = false;
        }

        // Установка нового текста
        public void NewText(string text)
        {
            IsPrinting = true;
            _currentPhraseFinal = text;
            _currentPhraseChars = _currentPhraseFinal.ToCharArray();
            _currentPhrase = "";
            StartCoroutine(PrintNextPhrase());
        }

        private IEnumerator PrintNextPhrase()
        {
            var prevChar = ' ';
            foreach (var currentChar in _currentPhraseChars)
            {
                yield return new WaitForSeconds(GetDelay(prevChar, currentChar));

                if (!char.IsWhiteSpace(currentChar))
                {
                    audioController.PlayTextSound(pitchRangeMin, pitchRangeMax);
                }

                if (!IsPrinting)
                {
                    yield break;
                }

                _currentPhrase += currentChar;
                _textBox.text = _currentPhrase;
                prevChar = currentChar;
            }

            IsPrinting = false;
        }

        // Возвращает значение задержки между печатью двух символов.
        // Задержка увеличена, если предыдущий символ - знак пунктуации или переход на новую строку.
        private float GetDelay(char prevChar, char nextChar)
        {
            if (char.IsPunctuation(prevChar) || nextChar == '\n')
            {
                return secondsBeforeNextSymbol * delayMultiplier;
            }

            return secondsBeforeNextSymbol;
        }
    }
}