using Common;
using Frames;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject authFields;
        public GameObject cardFields;
        public ImageController image;
        public TextBoxController text;
        
        private AuthController _auth;
        private CardInfoController _card;
        private ScenesController _scenes;

        private void Start()
        {
            _auth = GetComponent<AuthController>();
            _card = GetComponent<CardInfoController>();
            _scenes = GetComponent<ScenesController>();

            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    text.NewText("Давай давай вводи хорошего :)");
                    break;
                case State.Card:
                    OpenCardInfoForm();
                    break;
            }

            LeonidNorm();
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

            StateHolder.CurrentState = State.Final;
            _scenes.LoadAuthorsScene();
        }

        private void LeonidAngry()
        {
            image.NewImage("Final/leonid_angry");
        }
        
        private void LeonidNorm()
        {
            image.NewImage("Final/leonid_norm");
        }
    }
}