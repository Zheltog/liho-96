using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Frames
{
    public class MainController : MonoBehaviour
    {

        public TextBoxController textBox;
        public ImageController image;
        public AudioController player;
        public ChoicesController choices;

        private string _bossFightSceneName = "BossFightScene";
        private ScenesController _sceneController;
        
        private void Start()
        {
            if (!StateHolder.Initialized)
            {
                var jsonString = Resources.Load<TextAsset>("Text/framesConfig").text;
                var config = JsonConvert.DeserializeObject<Config>(jsonString);

                StateHolder.Init(config);
                StateHolder.SetFrame(config.StartingFrame);
                StateHolder.State = State.Frame;
            }

            _sceneController = GetComponent<ScenesController>();

            var music = StateHolder.CurrentFrame.Music ?? StateHolder.LastMusic;
            player.NewMusic(music);
            
            UpdateFrame();
            LogState();
        }

        private void Update()
        {
            if (SpaceOrEnterPressed() && !choices.WaitingForChoice)
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
            if (StateHolder.CurrentFrame.Type != FrameType.Choice)
            {
                Transition();
            }
        }

        private void Transition()
        {
            if (StateHolder.State != State.Frame) return;
            
            var currentFrame = StateHolder.CurrentFrame;

            if (currentFrame.Type != FrameType.Simple)
            {
                Debug.Log("Cannot transition - current frame is not simple");
                return;
            }
                
            Transition(currentFrame.Transition);
        }

        public void Transition(Transition transition)
        {
            StateHolder.UpdateFlags(transition.Actions);
            
            switch (transition.Type)
            {
                case TransitionType.Frame:
                {
                    var nextFrameName = transition.Next;
                    StateHolder.SetFrame(nextFrameName);
                    UpdateFrame();
                    break;
                }
                case TransitionType.Scene:
                {
                    // фрейм, который откроется, если из сцены вернутся на FrameScene
                    var returnFrame = transition.ReturnFrame;
                    if (returnFrame != null)
                    {
                        StateHolder.SetFrame(returnFrame);
                    }
                    
                    var nextSceneName = transition.Next;

                    if (nextSceneName == _bossFightSceneName)
                    {
                        Boss.StateHolder.Init(StateHolder.Flags);
                    }
                    
                    _sceneController.LoadSceneForName(nextSceneName);
                    break;
                }
                case TransitionType.Exit:
                    _sceneController.Exit();
                    break;
                case TransitionType.GameOver:
                    var endingName = transition.Next;
                    HandleGameOver(endingName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LogState();
        }

        private void LogState()
        {
            Debug.Log($"Frame: {StateHolder.CurrentFrameName}\n" +
                      $"Flags: {string.Join(", ", StateHolder.Flags)}");
        }

        private void UpdateFrame()
        {
            var currentFrame = StateHolder.CurrentFrame;
            
            textBox.NewText(currentFrame.Text);
            textBox.secondsBeforeNextSymbol = currentFrame.TextDelay ?? textBox.defaultSecondsBeforeNextSymbol;
            
            image.NewImage(currentFrame.Picture);

            var music = currentFrame.Music;
            if (music != null)
            {
                StateHolder.LastMusic = music;
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
            var availableChoices = ChoicesFilter.FilterChoices(choicesList);
            
            // если выбор только один - делаем кадр обычным, будет простой переход без кнопок
            if (availableChoices.Count == 1)
            {
                StateHolder.CurrentFrame.Type = FrameType.Simple;
                StateHolder.CurrentFrame.Transition = availableChoices[0].Transition;
            }
            else
            {
                // на всякий случай? чтобы точно не было перехода
                StateHolder.CurrentFrame.Type = FrameType.Choice;
                StateHolder.CurrentFrame.Transition = null;
                
                // показываем кнопки только после того, как текст напечатается
                yield return new WaitUntil(() => !textBox.IsPrinting);
                choices.NewChoices(availableChoices);
            }
        }
        
        private static bool SpaceOrEnterPressed()
        {
            return (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
                   && Input.anyKeyDown
                   && !Input.GetMouseButtonDown(0)
                   && !Input.GetMouseButtonDown(1)
                   && !Input.GetMouseButtonDown(2);
        }
        
        private void HandleGameOver(string endingName)
        {
            var ending = StateHolder.GameOvers[endingName];
            var currentImage = StateHolder.CurrentFrame.Picture;
            GameFinishedStateHolder.InitGameOver(ending.Text, ending.Picture, currentImage, ending.Sound, ending.Music);
            _sceneController.LoadGameOverScene();
        }
    }
}