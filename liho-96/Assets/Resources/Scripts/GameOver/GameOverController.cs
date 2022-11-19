using System.Collections;
using Frames;
using UnityEngine;

namespace Common
{
    public class GameOverController: MonoBehaviour
    {
        public TextBoxController text;
        public AudioController player;
        public ImageController image;
        public GameObject newGameButton;
        public GameObject loadGameButton;
        
        private ScenesController _scenes;
        private bool _initialized;

        private void Start()
        {
            StartCoroutine(SuspendedStart());
        }

        private void Update()
        {
            if (_initialized && !text.IsPrinting && !newGameButton.activeSelf)
            {
                newGameButton.SetActive(true);
                loadGameButton.SetActive(true);
            }

            if (Utils.SpaceOrEnterPressed() && text.IsPrinting)
            {
                text.FinishPrinting();
            }
        }

        public void NewGame()
        {
            _scenes.StartNewGame();
        }
        
        public void LoadSave()
        {
            _scenes.LoadFramesSceneSave();
        }

        public void FinishText()
        {
            text.FinishPrinting();
        }
        
        private IEnumerator SuspendedStart()
        {
            yield return new WaitForSeconds(0.1f);
            _scenes = GetComponent<ScenesController>();
            
            var gameOverText = GameFinishedStateHolder.GameOverText;
            if (gameOverText != null)
            {
                text.NewText(gameOverText);
            }

            var gameOverImage = GameFinishedStateHolder.GameOverImage;
            if (gameOverImage != null)
            {
                image.NewImage(gameOverImage, ImageController.NewImageLoadType.JustLighter);
            }
            
            var gameOverSound = GameFinishedStateHolder.GameOverSound;
            if (gameOverSound != null)
            {
                player.NewSound(gameOverSound);
            }
            
            var gameOverMusic = GameFinishedStateHolder.GameOverMusic;
            if (gameOverMusic != null)
            {
                player.NewMusic(gameOverMusic);
            }
            
            _initialized = true;
        }
    }
}