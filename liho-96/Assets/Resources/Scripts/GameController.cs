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
        GameStateHolder.SetFrame(gameStructure.StartingFrame);
        GameStateHolder.State = State.Frame;
        UpdateFrame();
        LogState();
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
        GameStateHolder.UpdateFlags(transition.Actions);
        
        switch (transition.Type)
        {
            case TransitionType.Frame:
            {
                var nextFrameName = transition.Next;
                GameStateHolder.SetFrame(nextFrameName);
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

        LogState();
    }

    private void LogState()
    {
        Debug.Log($"Frame: {GameStateHolder.CurrentFrameName}\nFlags: {string.Join(", ", GameStateHolder.Flags)}");
    }

    private void UpdateFrame()
    {
        var currentFrame = GameStateHolder.CurrentFrame;
        
        text.NewText(currentFrame.Text);
        text.secondsBeforeNextSymbol = currentFrame.TextDelay ?? text.defaultSecondsBeforeNextSymbol;
            
        image.NewImage(currentFrame.Picture);
        
        player.NewMusic(currentFrame.Music);
        player.NewSound(currentFrame.Sound);

        var choicesList = currentFrame.Choices;
        if (choicesList != null)
        {
            choices.NewChoices(ChoicesFilter.FilterChoices(currentFrame.Choices));
        }
        else
        {
            choices.SetActiveForButtons(false);
        }
    }
}