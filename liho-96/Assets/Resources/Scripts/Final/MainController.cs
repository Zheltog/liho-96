using TMPro;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject authFields;
        public GameObject cardFields;
        public TextMeshProUGUI text;

        private State _currentState = State.Auth;
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
            var authPassed = _auth.TryAuth();

            if (authPassed)
            {
                authFields.SetActive(false);
                _currentState = State.Card;
                text.text = "Нормально нормально. Раз такая тема пошла, мб и этого заполнишь? ;)))";
                cardFields.SetActive(true);
            }
        }

        private void ProcessCard()
        {
            var cardInfoPassed = _card.CheckInfo();

            if (cardInfoPassed)
            {
                Application.Quit();
            }
        }
        
        private enum State
        {
            Auth, Card
        }
    }
}