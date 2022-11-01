using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{

    public void LoadAuthorsScene()
    {
        SceneManager.LoadScene("Authors", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}