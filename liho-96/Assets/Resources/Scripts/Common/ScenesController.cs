using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ScenesController : MonoBehaviour
    {
        public void LoadFrameScene()
        {
            SceneManager.LoadScene("FrameScene", LoadSceneMode.Single);
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