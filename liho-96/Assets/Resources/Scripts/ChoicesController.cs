using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesController : MonoBehaviour
{
    public ButtonsBundle buttonsDouble;
    public ButtonsBundle buttonsTriple;
    
    public bool WaitingForChoice { get; private set; }

    private List<Choice> _currentChoices;
    private ChoiceType _currentType;
    
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void NewChoices(List<Choice> choices)
    {
        _currentType = choices.Count switch
        {
            2 => ChoiceType.DOUBLE,
            3 => ChoiceType.TRIPLE,
            _ => throw new Exception("should be 2 or 3 choices")
        };

        _currentChoices = choices;

        SetActiveForButtons(true);
    }

    public void ChooseFirst()
    {
        SetActiveForButtons(false);
        _gameController.Transition(_currentChoices[0].Transition);
    }
    
    public void ChooseSecond()
    {
        SetActiveForButtons(false);
        _gameController.Transition(_currentChoices[1].Transition);
    }

    public void ChooseThird()
    {
        SetActiveForButtons(false);
        _gameController.Transition(_currentChoices[2].Transition);
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
            case ChoiceType.DOUBLE:
                buttonsDouble.SetActive(true);
                buttonsDouble.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text);
                break;
            case ChoiceType.TRIPLE:
                buttonsTriple.SetActive(true);
                buttonsTriple.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text, _currentChoices[2].Text);
                break;
        }
    }

    private enum ChoiceType
    {
        DOUBLE,
        TRIPLE
    }
}