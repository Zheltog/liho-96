using Common;
using Frames;
using UnityEngine;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public GameObject cardFields;
        public GameObject nextButton;
        public GameObject skipButton;
        public ImageController image;
        public TextBoxController text;
        
        private CardInfoController _card;
        private ScenesController _scenes;

        private void Start()
        {
            _card = GetComponent<CardInfoController>();
            _scenes = GetComponent<ScenesController>();
            text.NewText(CommentsHolder.CardInfo);
            LeonidNorm();
            SceneStateHolder.LastSavableSceneState = SceneState.Final;
        }

        private void Update()
        {
            if (text.IsPrinting) return;

            if (cardFields.activeSelf) return;
            
            cardFields.SetActive(true);

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
            
            _card.CheckInfo();
        }

        public void Skip()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
                return;
            }
            
            Finish();
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

        public void Finish()
        {
            SceneStateHolder.LastSavableSceneState = SceneState.Frame;
            _scenes.LoadFramesScene();
            GameFinishedController.IsGameFinished = true;
            
            nextButton.SetActive(false);
            skipButton.SetActive(false);
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