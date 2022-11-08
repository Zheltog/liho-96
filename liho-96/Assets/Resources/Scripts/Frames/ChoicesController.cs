using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frames
{
    public class ChoicesController : MonoBehaviour
    {
        // TODO: менее хардкодово?
        public ButtonsBundle buttonsDouble;
        public ButtonsBundle buttonsTriple;
        public MainController mainController;
        public bool WaitingForChoice { get; private set; }

        private List<Choice> _currentChoices;
        private ChoiceType _currentType;

        public void NewChoices(List<Choice> choices)
        {
            _currentType = choices.Count switch
            {
                2 => ChoiceType.Double,
                3 => ChoiceType.Triple,
                _ => throw new Exception("should be 2 or 3 choices")
            };

            _currentChoices = choices;

            SetActiveForButtons(true);
        }

        public void ChooseFirst()
        {
            Choose(0);
        }

        public void ChooseSecond()
        {
            Choose(1);
        }

        public void ChooseThird()
        {
            Choose(2);
        }

        private void Choose(int choiceIndex)
        {
            SetActiveForButtons(false);
            mainController.Transition(_currentChoices[choiceIndex].Transition);
        }

        private void SetActiveForButtons(bool isActive)
        {
            WaitingForChoice = isActive;

            if (!isActive)
            {
                buttonsDouble.SetActive(false);
                buttonsTriple.SetActive(false);
                return;
            }

            switch (_currentType)
            {
                case ChoiceType.Double:
                    buttonsDouble.SetActive(true);
                    buttonsDouble.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text);
                    break;
                case ChoiceType.Triple:
                    buttonsTriple.SetActive(true);
                    buttonsTriple.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text,
                        _currentChoices[2].Text);
                    break;
            }
        }

        private enum ChoiceType
        {
            Double,
            Triple
        }
    }
}