using System.Collections;
using TMPro;
using UnityEngine;

public class FrameTextController : MonoBehaviour
{

    public float secondsBeforeNextSymbol = 0.075f;

    public float delayMultiplier = 5;

    private char[] _currentPhraseChars;

    private string _currentPhrase;

    private string _currentPhraseFinal;

    private bool _isPrinting;

    private TextMeshProUGUI _textBox;

    private GameController _gameController;

    private void Start()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void OnClick()
    {
        // Печатает весь текст, если он ещё не был отображен полностью.
        if (_isPrinting)
        {
            _textBox.text = _currentPhraseFinal;
            _isPrinting = false;
            return;
        }

        // Запускает переход, если у кадра не было вариантов выбора
        if (GameStateHolder.CurrentFrame.Type != FrameType.Choice)
        {
            _gameController.Transition();
        }
    }

    // Установка нового текста. Вызывается из игрового контроллера
    public void NewText(string text)
    {
        _currentPhraseFinal = text;
        _currentPhraseChars = _currentPhraseFinal.ToCharArray();
        _currentPhrase = "";
        _isPrinting = true;
        StartCoroutine(PrintNextPhrase());
    }

    private IEnumerator PrintNextPhrase()
    {
        var prevChar = ' ';
        foreach (var currentChar in _currentPhraseChars)
        {
            yield return new WaitForSeconds(GetDelay(prevChar, currentChar));
            if (!_isPrinting)
            {
                yield break;
            }
            _currentPhrase += currentChar;
            _textBox.text = _currentPhrase;
            prevChar = currentChar;
        }

        _isPrinting = false;
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
