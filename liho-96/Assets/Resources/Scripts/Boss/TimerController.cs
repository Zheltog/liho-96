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
        private Animator _animator;
        private float _timeRemainingBeforeEnd;
        private bool _redAlert;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _animator = GetComponent<Animator>();
            _timeRemainingBeforeEnd = fightSeconds;
        }

        private void Update()
        {
            if (_timeRemainingBeforeEnd <= 0)
            {
                // TODO: нормальный текст
                mainController.GameOver(GameOverCommentsHolder.TimerRanOut);
                return;
            }
            
            _timeRemainingBeforeEnd -= Time.deltaTime;
            SetTimeRemaining(_timeRemainingBeforeEnd);
        }

        public void AddTime(float seconds)
        {
            _timeRemainingBeforeEnd += seconds;
            SetTimeRemaining(_timeRemainingBeforeEnd);
            Pulse();
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
                Pulse();
            }

            if (_redAlert && secondsRemaining > redAlertSeconds)
            {
                _redAlert = false;
                _text.color = new Color(1f, 1f, 1f);
            }
        }

        private void Pulse()
        {
            _animator.Play("TimerPulse");
        }
    }
}