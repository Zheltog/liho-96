using System.Collections;
using UnityEngine;

namespace Common
{
    public class GameOverController: MonoBehaviour
    {
        public TextBoxController text;
        public AudioController player;
        
        private ScenesController _scenes;

        private void Start()
        {
            StartCoroutine(SuspendedStart());
        }

        public void NewGame()
        {
            Frames.StateHolder.Initialized = false;
            _scenes.LoadFramesScene();
        }

        public void FinishText()
        {
            text.FinishPrinting();
        }
        
        private IEnumerator SuspendedStart()
        {
            yield return new WaitForSeconds(0.1f);
            _scenes = GetComponent<ScenesController>();
            var gameOverComment = GameFinishedStateHolder.GameOverComment;
            if (gameOverComment != null)
            {
                text.NewText(gameOverComment);
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
        }
    }
}