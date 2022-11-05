using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FramesController : MonoBehaviour
{

    public TextBoxController textBox;
    public ImageController image;
    public AudioController player;
    public FramesChoicesController framesChoices;

    private void Start()
    {
        if (!FramesStateHolder.Initialized)
        {
            var jsonString = Resources.Load<TextAsset>("Text/framesConfig").text;
            var framesConfig = JsonConvert.DeserializeObject<FramesConfig>(jsonString);

            FramesStateHolder.Init(framesConfig);
            FramesStateHolder.SetFrame(framesConfig.StartingFrame);
            FramesStateHolder.State = State.Frame;
        }

        player.NewMusic(FramesStateHolder.LastMusic);
        
        UpdateFrame();
        LogState();
    }

    private void Update()
    {
        if (AnyKeyIgnoreMouse() && !framesChoices.WaitingForChoice)
        {
            NextFrame();
        }
    }

    public void NextFrame()
    {
        // Печатает весь текст, если он ещё не был отображен полностью.
        if (textBox.IsPrinting)
        {
            textBox.FinishPrinting();
            return;
        }
            
        // Запускает переход, если у кадра не было вариантов выбора
        if (FramesStateHolder.CurrentFrame.Type != FrameType.Choice)
        {
            Transition();
        }
    }

    public void Transition()
    {
        if (FramesStateHolder.State == State.Frame)
        {
            var currentFrame = FramesStateHolder.CurrentFrame;

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
        FramesStateHolder.UpdateFlags(transition.Actions);
        
        switch (transition.Type)
        {
            case TransitionType.Frame:
            {
                var nextFrameName = transition.Next;
                FramesStateHolder.SetFrame(nextFrameName);
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
        Debug.Log($"Frame: {FramesStateHolder.CurrentFrameName}\nFlags: {string.Join(", ", FramesStateHolder.Flags)}");
    }

    private void UpdateFrame()
    {
        var currentFrame = FramesStateHolder.CurrentFrame;
        
        textBox.NewText(currentFrame.Text);
        textBox.secondsBeforeNextSymbol = currentFrame.TextDelay ?? textBox.defaultSecondsBeforeNextSymbol;
            
        image.NewImage(currentFrame.Picture);

        var music = currentFrame.Music;

        if (music != null)
        {
            FramesStateHolder.LastMusic = music;
        }

        player.NewMusic(music);
        player.NewSound(currentFrame.Sound);

        var choicesList = currentFrame.Choices;
        if (choicesList != null)
        {
            StartCoroutine(UpdateChoices(choicesList));
        }
    }

    private IEnumerator UpdateChoices(List<Choice> choicesList)
    {
        yield return new WaitUntil(() => !textBox.IsPrinting);
        framesChoices.NewChoices(FramesChoicesFilter.FilterChoices(choicesList));
    }
    
    private static bool AnyKeyIgnoreMouse()
    {
        return Input.anyKeyDown
               && !Input.GetMouseButtonDown(0)
               && !Input.GetMouseButtonDown(1)
               && !Input.GetMouseButtonDown(2);
    }
}