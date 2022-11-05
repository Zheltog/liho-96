using TMPro;
using UnityEngine;

namespace Boss
{
    public class TimerController : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void SetTimeRemaining(float secondsRemaining)
        {
            float minutes = Mathf.FloorToInt(secondsRemaining / 60);
            float seconds = Mathf.FloorToInt(secondsRemaining % 60);
            _text.text = $"{minutes:00}:{seconds:00}";
        }
    }
}