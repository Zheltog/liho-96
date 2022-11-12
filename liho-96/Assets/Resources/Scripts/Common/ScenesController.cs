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
                // TODO кал
                case SceneState.Final:
                    if (Final.StateHolder.CurrentState == State.Quit)
                    {
                        Exit();
                    }
                    else
                    {
                        SceneManager.LoadScene(
                            Final.StateHolder.CurrentState == State.Final ? "FrameScene" : "FinalScene",
                            LoadSceneMode.Single);
                    }
                    break;
                default:
                    SceneManager.LoadScene("FrameScene", LoadSceneMode.Single);
                    break;
            }
        }

        public void LoadAuthorsScene()
        {
            SceneManager.LoadScene("AuthorsScene", LoadSceneMode.Single);
        }

        public void Exit()
        {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }
}