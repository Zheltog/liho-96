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
        public Vector2 nextButtonPositionAfterFirstError = new Vector2(-150, -200);
        
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
            if (text.IsPrinting || cardFields.activeSelf) return;
            cardFields.SetActive(true);
            nextButton.SetActive(true);
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

            if (skipButton.activeSelf) return;
            
            skipButton.SetActive(true);
            nextButton.transform.localPosition = nextButtonPositionAfterFirstError;
        }

        public void Finish()
        {
            _scenes.LoadFramesScene();
            GameFinishedStateHolder.IsGameFinished = true;
        }

        private void LeonidAngry()
        {
            image.NewImage("leonid_angry", ImageController.NewImageLoadType.DarkerThenLighter);
        }
        
        private void LeonidNorm()
        {
            image.NewImage("leonid_norm", ImageController.NewImageLoadType.DarkerThenLighter);
        }
    }
}