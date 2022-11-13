using Common;
using Frames;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject authFields;
        public GameObject cardFields;
        public GameObject nextButton;
        public GameObject skipButton;
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
                    text.NewText(CommentsHolder.CardIfAuthSkipped);
                    break;
                case State.AuthPassed:
                    text.NewText(CommentsHolder.CardIfAuthPassed);
                    break;
            }

            LeonidNorm();
            SceneStateHolder.LastSavableSceneState = SceneState.Final;
        }

        private void Update()
        {
            if (text.IsPrinting) return;

            switch (StateHolder.CurrentState)
            {
                case State.Auth:
                    if (authFields.activeSelf) return;
                    OpenAuthForm();
                    break;
                case State.AuthSkipped:
                case State.AuthPassed:
                    if (cardFields.activeSelf) return;
                    OpenCardInfoForm();
                    break;
            }

            if (!nextButton.activeSelf)
            {
                nextButton.SetActive(true);
            }
            
            if (!skipButton.activeSelf)
            {
                skipButton.SetActive(true);
            }
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
                    text.NewText(CommentsHolder.CardIfAuthSkipped);
                    CloseAuthForm();
                    break;
                case State.AuthSkipped:
                    SceneStateHolder.LastSavableSceneState = SceneState.Frame;
                    _scenes.LoadPreviousScene();
                    GameFinishedController.IsGameFinished = true;
                    break;
                case State.AuthPassed:
                    Error(CommentsHolder.NoSkipCardIfNoAuth);
                    return;
            }
            
            nextButton.SetActive(false);
            skipButton.SetActive(false);
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
                    text.NewText(CommentsHolder.CardIfAuthPassed);
                    CloseAuthForm();
                    break;
                case State.AuthSkipped:
                case State.AuthPassed:
                    SceneStateHolder.LastSavableSceneState = SceneState.Frame;
                    _scenes.LoadFramesScene();
                    GameFinishedController.IsGameFinished = true;
                    break;
            }
            
            nextButton.SetActive(false);
            skipButton.SetActive(false);
        }

        private void OpenAuthForm()
        {
            LeonidNorm();
            authFields.SetActive(true);
            cardFields.SetActive(false);
        }

        private void CloseAuthForm()
        {
            LeonidNorm();
            authFields.SetActive(false);
        }

        private void OpenCardInfoForm()
        {
            LeonidNorm();
            authFields.SetActive(false);
            cardFields.SetActive(true);
        }

        private void LeonidAngry()
        {
            image.NewImage("leonid_angry");
        }
        
        private void LeonidNorm()
        {
            image.NewImage("leonid_norm");
        }
    }
}