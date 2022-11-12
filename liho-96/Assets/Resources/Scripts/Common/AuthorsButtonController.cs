using TMPro;
using UnityEngine;

namespace Common
{
    public class AuthorsButtonController : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        public Color colorOnMouseOver = Color.white;

        private Color _originalColor;

        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _originalColor = _text.color;
        }
        
        public void OnMouseOver()
        {
            _text.color = colorOnMouseOver;
        }

        public void OnMouseExit()
        {
            _text.color = _originalColor;
        }
    }
}