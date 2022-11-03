using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class FrameTextController : MonoBehaviour
{

    public float secondsBeforeNextSymbol = 0.075f;

    public float delayMultiplier = 5;

    private Dictionary<string, Frame> _frames;
    
    private Frame _startFrame;

    private Frame _currentFrame;

    private char[] _currentPhraseChars;

    private string _currentPhrase;

    private string _currentPhraseFinal;

    private bool _isPrinting;

    private TextMeshProUGUI _textBox;

    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
        var structure = JsonConvert.DeserializeObject<GameStructure>(jsonString);

        _frames = structure.Frames;
        _startFrame = _frames[structure.StartingFrame];

        _textBox = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdatePhrase();
        }
    }

    private void UpdatePhrase()
    {
        if (_isPrinting)
        {
            _textBox.text = _currentPhraseFinal;
            _isPrinting = false;
            return;
        }

        _currentFrame = NextFrame();
        _currentPhraseFinal = _currentFrame.Text;
        
        Debug.Log(_currentPhraseFinal);
        
        _currentPhraseChars = _currentPhraseFinal.ToCharArray();
        _currentPhrase = "";
        _isPrinting = true;
        StartCoroutine(PrintNextPhrase());
    }

    private Frame NextFrame()
    {
        if (_currentFrame == null)
        {
            return _startFrame;
        }

        return _frames[_currentFrame.Transition.Next];
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
