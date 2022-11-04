using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public FrameTextController text;
    public ImageController image;

    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
        var gameStructure = JsonConvert.DeserializeObject<GameStructure>(jsonString);

        GameStateHolder.Init(gameStructure);
        GameStateHolder.CurrentFrame = GameStateHolder.StartFrame;
        GameStateHolder.State = State.Frame;
        UpdateFrame();
    }

    public void SimpleTransition()
    {
        if (GameStateHolder.State == State.Frame)
        {
            var currentFrame = GameStateHolder.CurrentFrame;

            if (currentFrame.Type != FrameType.Simple)
            {
                Debug.Log("Cannot transition - current frame is not simple");
                return;
            }
            
            switch (currentFrame.Transition.Type)
            {
                case TransitionType.Frame:
                {
                    var nextFrameName = currentFrame.Transition.Next;
                    GameStateHolder.CurrentFrame = GameStateHolder.Frames[nextFrameName];
                    UpdateFrame();
                    break;
                }
                case TransitionType.Scene:
                {
                    var nextSceneName = currentFrame.Transition.Next;
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
                    break;
                }
                case TransitionType.Exit:
                    Application.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void UpdateFrame()
    {
        text.NewText();
        image.NewImage();
    }
}