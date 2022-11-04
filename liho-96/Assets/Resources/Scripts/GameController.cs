using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public FrameTextController text;
    public ImageController image;
    public AudioController player;
    public ChoicesController choices;

    private void Start()
    {
        var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
        var gameStructure = JsonConvert.DeserializeObject<GameStructure>(jsonString);

        GameStateHolder.Init(gameStructure);
        GameStateHolder.CurrentFrame = GameStateHolder.StartFrame;
        GameStateHolder.State = State.Frame;
        UpdateFrame();
    }

    public void Transition()
    {
        if (GameStateHolder.State == State.Frame)
        {
            var currentFrame = GameStateHolder.CurrentFrame;

            if (currentFrame.Type != FrameType.Simple)
            {
                Debug.Log("Cannot transition - current frame is not simple");
                return;
            }
            
            Transition(currentFrame.Transition);
        }
    }

    public void Transition(Transition transition)
    {
        switch (transition.Type)
        {
            case TransitionType.Frame:
            {
                var nextFrameName = transition.Next;
                GameStateHolder.CurrentFrame = GameStateHolder.Frames[nextFrameName];
                UpdateFrame();
                break;
            }
            case TransitionType.Scene:
            {
                var nextSceneName = transition.Next;
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

    private void UpdateFrame()
    {
        text.NewText();
        image.NewImage();
        player.NewAudio();

        var choicesList = GameStateHolder.CurrentFrame.Choices;
        if (choicesList != null)
        {
            choices.NewChoices(choicesList);
        }
    }
}