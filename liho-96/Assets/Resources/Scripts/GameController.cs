using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
        var gameStructure = JsonConvert.DeserializeObject<GameStructure>(jsonString);

        GameStateHolder.Init(gameStructure);
    }

    public void SimpleTransition()
    {
        if (GameStateHolder.State == State.Start)
        {
            GameStateHolder.CurrentFrame = GameStateHolder.StartFrame;
            GameStateHolder.State = State.Frame;
        }
        else if (GameStateHolder.State == State.Frame)
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
}