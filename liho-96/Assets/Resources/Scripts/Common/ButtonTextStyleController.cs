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
        
        public void SetUnderline()
        {
            _text.fontStyle = FontStyles.Underline;
        }

        public void SetNormal()
        {
            _text.fontStyle = FontStyles.Normal;
        }
    }
}