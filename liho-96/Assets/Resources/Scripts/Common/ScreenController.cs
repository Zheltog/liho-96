using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public int resolutionX = 640;
    public int resolutionY = 480;
    
    private void Start()
    {
        Screen.SetResolution(resolutionX, resolutionY, true);
    }
}