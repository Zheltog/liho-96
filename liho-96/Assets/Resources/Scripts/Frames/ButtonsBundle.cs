using Common;
using TMPro;
using UnityEngine;

namespace Frames
{
    public class ButtonsBundle : MonoBehaviour
    {
        public TextMeshProUGUI textOnButton1;
        public TextMeshProUGUI textOnButton2;
        public TextMeshProUGUI textOnButton3;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetTexts(string text1, string text2)
        {
            textOnButton1.text = Utils.FormatButtonText(text1);
            textOnButton2.text = Utils.FormatButtonText(text2);
        }

        public void SetTexts(string text1, string text2, string text3)
        {
            textOnButton1.text = Utils.FormatButtonText(text1);
            textOnButton2.text = Utils.FormatButtonText(text2);
            textOnButton3.text = Utils.FormatButtonText(text3);
        }
    }
}