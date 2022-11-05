using System;
using System.Collections.Generic;
using UnityEngine;

public class FramesChoicesController : MonoBehaviour
{
    public FramesButtonsBundle framesButtonsDouble;
    public FramesButtonsBundle framesButtonsTriple;
    
    public bool WaitingForChoice { get; private set; }

    private List<Choice> _currentChoices;
    private ChoiceType _currentType;
    
    private FramesController _framesController;

    private void Start()
    {
        _framesController = GameObject.FindWithTag("GameController").GetComponent<FramesController>();
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
        _framesController.Transition(_currentChoices[0].Transition);
    }
    
    public void ChooseSecond()
    {
        SetActiveForButtons(false);
        _framesController.Transition(_currentChoices[1].Transition);
    }

    public void ChooseThird()
    {
        SetActiveForButtons(false);
        _framesController.Transition(_currentChoices[2].Transition);
    }

    private void SetActiveForButtons(bool isActive)
    {
        WaitingForChoice = isActive;
        
        if (!isActive)
        {
            framesButtonsDouble.SetActive(false);
            framesButtonsTriple.SetActive(false);
            return;
        }
        
        switch (_currentType)
        {
            case ChoiceType.DOUBLE:
                framesButtonsDouble.SetActive(true);
                framesButtonsDouble.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text);
                break;
            case ChoiceType.TRIPLE:
                framesButtonsTriple.SetActive(true);
                framesButtonsTriple.SetTexts(_currentChoices[0].Text, _currentChoices[1].Text, _currentChoices[2].Text);
                break;
        }
    }

    private enum ChoiceType
    {
        DOUBLE,
        TRIPLE
    }
}