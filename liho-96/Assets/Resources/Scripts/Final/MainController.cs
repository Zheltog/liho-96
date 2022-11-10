using TMPro;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject authFields;
        public GameObject cardFields;
        public TextMeshProUGUI text;
        public Animator imageAnimator;

        private State _currentState = State.Auth;
        private LeonidState _currentLeonidState = LeonidState.Norm;
        private AuthController _auth;
        private CardInfoController _card;

        private void Start()
        {
            _auth = GetComponent<AuthController>();
            _card = GetComponent<CardInfoController>();
        }

        public void OnButtonClick()
        {
            switch (_currentState)
            {
                case State.Auth:
                    ProcessAuth();
                    break;
                case State.Card:
                    ProcessCard();
                    break;
            }
        }
        
        private void ProcessAuth()
        {
            var authError = _auth.TryAuthError();

            if (authError != null)
            {
                text.text = authError;
                LeonidAngry();
                return;
            }

            LeonidNorm();
            authFields.SetActive(false);
            _currentState = State.Card;
            text.text = "Нормально нормально. Раз такая тема пошла, мб и этого заполнишь? ;)))";
            cardFields.SetActive(true);
        }

        private void ProcessCard()
        {
            var cardInfoPassed = _card.CheckInfo();

            if (cardInfoPassed)
            {
                Application.Quit();
            }
        }

        private void LeonidAngry()
        {
            if (_currentLeonidState != LeonidState.Angry)
            {
                imageAnimator.Play("ToLeonidAngry");
                _currentLeonidState = LeonidState.Angry;
            }
        }
        
        private void LeonidNorm()
        {
            if (_currentLeonidState != LeonidState.Norm)
            {
                imageAnimator.Play("ToLeonidNorm");
                _currentLeonidState = LeonidState.Norm;
            }
        }
        
        private enum State
        {
            Auth, Card
        }

        private enum LeonidState
        {
            Norm, Angry
        }
    }
}