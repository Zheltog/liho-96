using Common;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject authFields;
        public GameObject cardFields;
        public Animator imageAnimator;
        public TextBoxController text;
        
        private LeonidState _currentLeonidState = LeonidState.Norm;
        private AuthController _auth;
        private CardInfoController _card;

        private void Start()
        {
            _auth = GetComponent<AuthController>();
            _card = GetComponent<CardInfoController>();

            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    text.NewText("Давай давай вводи хорошего :)");
                    break;
                case State.Card:
                    OpenCardInfoForm();
                    break;
            }

            SceneStateHolder.LastSavableSceneState = SceneState.Final;
        }

        public void OnButtonClick()
        {
            if (text.IsPrinting)
            {
                return;
            }
            
            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    ProcessAuth();
                    break;
                case State.Card:
                    ProcessCard();
                    break;
            }
        }

        public void ClickOnText()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
        }
        
        private void ProcessAuth()
        {
            var authError = _auth.TryAuthError();

            if (authError != null)
            {
                text.NewText(authError);
                LeonidAngry();
                return;
            }
            
            OpenCardInfoForm();
        }

        private void OpenCardInfoForm()
        {
            LeonidNorm();
            authFields.SetActive(false);
            text.NewText("Нормально нормально. Раз такая тема пошла, мб и этого заполнишь? ;)))");
            cardFields.SetActive(true);
            StateHolder.CurrentState = State.Card;
        }

        private void ProcessCard()
        {
            var cardError = _card.CheckInfo();

            if (cardError != null)
            {
                text.NewText(cardError);
                LeonidAngry();
                return;
            }

            Debug.Log("QUIT");
            Application.Quit();
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

        private enum LeonidState
        {
            Norm, Angry
        }
    }
}