using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{

    public float secondsBeforeNextSymbol = 0.1f;

    private string[] _phrases =
    {
        "Ноябрь, 1996 год. Дорога к небольшому провинциальному городу ведет через лес. Одинокая машина едет под проливным дождём.",
        "Молчаливый курьер везёт груз. Он никогда не спрашивает, что и кому он доставляет. За это его и ценят - и, возможно, лишь поэтому он до сих пор не получил пулю в лоб.",
        "А это вторая фраза.",
        "А вот и третья (последняя). И длинннннная штууууууууууууууууууууууууууууууууууууууууууууука (по приколу).\n\nИ перенос строки.......",
        "Да?"
    };

    private int _currentPhraseIndex;

    private char[] _currentPhraseChars;

    private string _currentPhrase;

    private string _currentPhraseFinal;

    private bool _isPrinting;

    private TextMeshProUGUI _text;

    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
        var structure = JsonConvert.DeserializeObject<GameStructure>(jsonString);
        _phrases = structure.Frames.Values.Select(s => s.Text).ToArray();
        
        _text = GetComponent<TextMeshProUGUI>();
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
            _text.text = _currentPhraseFinal;
            _isPrinting = false;
            return;
        }
        
        _currentPhraseFinal = _phrases[_currentPhraseIndex];
        Debug.Log(_currentPhraseFinal);
        
        _currentPhraseChars = _currentPhraseFinal.ToCharArray();
        _currentPhrase = "";
        _currentPhraseIndex++;
        _isPrinting = true;
        StartCoroutine(PrintNextPhrase());
    }

    private IEnumerator PrintNextPhrase()
    {
        foreach (var currentChar in _currentPhraseChars)
        {
            yield return new WaitForSeconds(secondsBeforeNextSymbol);
            if (!_isPrinting)
            {
                yield break;
            }
            _currentPhrase += currentChar;
            _text.text = _currentPhrase;
        }
    }
}