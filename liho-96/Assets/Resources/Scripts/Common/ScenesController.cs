using Final;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ScenesController : MonoBehaviour
    {
        public void LoadPreviousScene()
        {
            switch (SceneStateHolder.LastSavableSceneState)
            {
                case SceneState.Frame:
                    SceneManager.LoadScene("FrameScene", LoadSceneMode.Single);
                    break;
                case SceneState.Final:
                    if (Final.StateHolder.CurrentState == State.Final)
                    {
                        Debug.Log("QUIT");
                        Application.Quit();
                    }
                    else
                    {
                        SceneManager.LoadScene("FinalScene", LoadSceneMode.Single);
                    }
                    break;
            }
        }

        public void LoadBossFightScene()
        {
            // TODO
        }

        public void LoadAuthorsScene()
        {
            SceneManager.LoadScene("AuthorsScene", LoadSceneMode.Single);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}