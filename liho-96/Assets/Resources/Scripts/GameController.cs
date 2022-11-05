using System;
using System.Collections;
using System.Collections.Generic;
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
        if (!GameStateHolder.Initialized)
        {
            var jsonString = Resources.Load<TextAsset>("Text/gameStructure").text;
            var gameStructure = JsonConvert.DeserializeObject<GameStructure>(jsonString);

            GameStateHolder.Init(gameStructure);
            GameStateHolder.SetFrame(gameStructure.StartingFrame);
            GameStateHolder.State = State.Frame;
        }
        
        UpdateFrame();
        LogState();
    }

    private void Update()
    {
        if (AnyKeyIgnoreMouse() && !choices.WaitingForChoice)
        {
            NextFrame();
        }
    }

    public void NextFrame()
    {
        // Печатает весь текст, если он ещё не был отображен полностью.
        if (text.IsPrinting)
        {
            text.FinishPrinting();
            return;
        }
            
        // Запускает переход, если у кадра не было вариантов выбора
        if (GameStateHolder.CurrentFrame.Type != FrameType.Choice)
        {
            Transition();
        }
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
            StartCoroutine(UpdateChoices(choicesList));
        }
    }

    private IEnumerator UpdateChoices(List<Choice> choicesList)
    {
        yield return new WaitUntil(() => !text.IsPrinting);
        choices.NewChoices(ChoicesFilter.FilterChoices(choicesList));
    }
    
    private static bool AnyKeyIgnoreMouse()
    {
        return Input.anyKeyDown
               && !Input.GetMouseButtonDown(0)
               && !Input.GetMouseButtonDown(1)
               && !Input.GetMouseButtonDown(2);
    }
}