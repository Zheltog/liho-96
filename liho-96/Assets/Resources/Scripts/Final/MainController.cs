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
                    text.NewText(CommentsHolder.Auth);
                    break;
                case State.AuthSkipped:
                    OpenCardInfoForm(CommentsHolder.CardIfAuthSkipped);
                    break;
                case State.AuthPassed:
                    OpenCardInfoForm(CommentsHolder.CardIfAuthPassed);
                    break;
            }

            LeonidNorm();
            SceneStateHolder.LastSavableSceneState = SceneState.Final;
        }

        public void Next()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
                return;
            }
            
            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    _auth.TryAuth();
                    break;
                case State.AuthSkipped:
                    _card.CheckInfo();
                    break;
                case State.AuthPassed:
                    _card.CheckInfo();
                    break;
            }
        }

        public void Skip()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
                return;
            }
            
            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    StateHolder.CurrentState = State.AuthSkipped;
                    OpenCardInfoForm(CommentsHolder.CardIfAuthSkipped);
                    break;
                case State.AuthSkipped:
                case State.AuthPassed:
                    StateHolder.CurrentState = State.Final;
                    _scenes.LoadPreviousScene();
                    StateHolder.CurrentState = State.Quit;
                    break;
            }
        }

        public void ClickOnText()
        {
            text.FinishPrinting();
        }

        public void Error(string comment)
        {
            text.NewText(comment);
            LeonidAngry();
        }

        public void Success()
        {
            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    StateHolder.CurrentState = State.AuthPassed;
                    OpenCardInfoForm(CommentsHolder.CardIfAuthPassed);
                    break;
                case State.AuthSkipped:
                    StateHolder.CurrentState = State.Final;
                    _scenes.LoadAuthorsScene();
                    break;
                case State.AuthPassed:
                    StateHolder.CurrentState = State.Final;
                    _scenes.LoadAuthorsScene();
                    break;
            }
        }

        private void OpenCardInfoForm(string comment)
        {
            LeonidNorm();
            authFields.SetActive(false);
            text.NewText(comment);
            cardFields.SetActive(true);
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