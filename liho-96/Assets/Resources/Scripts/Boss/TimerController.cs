using TMPro;
using UnityEngine;

namespace Boss
{
    public class TimerController : MonoBehaviour
    {
        public MainController mainController;
        public float fightSeconds = 30;
        public float redAlertSeconds = 10;
        
        private TextMeshProUGUI _text;
        private float _timeRemainingBeforeEnd;
        private bool _redAlert;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _timeRemainingBeforeEnd = fightSeconds;
        }

        private void Update()
        {
            if (_timeRemainingBeforeEnd <= 0)
            {
                mainController.GameOver("Коммунисты завели жигу и уехали в рассвет. С ЛИХО...");
                return;
            }
            
            _timeRemainingBeforeEnd -= Time.deltaTime;
            SetTimeRemaining(_timeRemainingBeforeEnd);
        }

        public void AddTime(float seconds)
        {
            _timeRemainingBeforeEnd += seconds;
            SetTimeRemaining(_timeRemainingBeforeEnd);
        }

        private void SetTimeRemaining(float secondsRemaining)
        {
            float minutes = Mathf.FloorToInt(secondsRemaining / 60);
            float seconds = Mathf.FloorToInt(secondsRemaining % 60);
            _text.text = $"{minutes:00}:{seconds:00}";

            if (!_redAlert && secondsRemaining <= redAlertSeconds)
            {
                _redAlert = true;
                _text.color = new Color(1f, 0f, 0f);
            }

            if (_redAlert && secondsRemaining > redAlertSeconds)
            {
                _redAlert = true;
                _text.color = new Color(1f, 1f, 1f);
            }
        }
    }
}