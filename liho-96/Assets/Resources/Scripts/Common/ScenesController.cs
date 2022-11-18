using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ScenesController : MonoBehaviour
    {
        public void LoadPreviousScene()
        {
            if (GameFinishedStateHolder.IsGameFinished)
            {
                Exit();
                return;
            }
            switch (SceneStateHolder.LastSavableSceneState)
            {
                case SceneState.Final:
                    LoadFinalScene();
                    break;
                default:
                    LoadFramesScene();
                    break;
            }
        }

        public void LoadSceneForName(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LoadFramesScene()
        {
            SceneManager.LoadScene("FrameScene", LoadSceneMode.Single);
        }
        
        public void LoadFinalScene()
        {
            SceneManager.LoadScene("FinalScene", LoadSceneMode.Single);
        }

        public void LoadAuthorsScene()
        {
            SceneManager.LoadScene("AuthorsScene", LoadSceneMode.Single);
        }

        public void LoadGameOverScene()
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }

        public void Exit()
        {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }
}