using TMPro;
using UnityEngine;

namespace Common
{
    public class ButtonTextStyleController : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public void OnMouseOver()
        {
            _text.fontStyle = FontStyles.Underline;
        }

        public void OnMouseExit()
        {
            _text.fontStyle = FontStyles.Normal;
        }
    }
}