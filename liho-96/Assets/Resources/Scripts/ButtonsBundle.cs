using TMPro;
using UnityEngine;

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
        textOnButton1.text = text1;
        textOnButton2.text = text2;
    }
    
    public void SetTexts(string text1, string text2, string text3)
    {
        textOnButton1.text = text1;
        textOnButton2.text = text2;
        textOnButton3.text = text3;
    }
}