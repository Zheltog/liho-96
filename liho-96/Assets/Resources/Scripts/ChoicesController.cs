using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesController : MonoBehaviour
{
    public GameObject buttonsDouble;
    public GameObject buttonsTriple;

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
        _gameController.Transition(_currentChoices[0].Transition);
        SetActiveForButtons(false);
    }
    
    public void ChooseSecond()
    {
        _gameController.Transition(_currentChoices[1].Transition);
        SetActiveForButtons(false);
    }

    public void ChooseThird()
    {
        _gameController.Transition(_currentChoices[2].Transition);
        SetActiveForButtons(false);
    }

    private void SetActiveForButtons(bool isActive)
    {
        switch (_currentType)
        {
            case ChoiceType.DOUBLE:
                buttonsDouble.SetActive(isActive);
                break;
            case ChoiceType.TRIPLE:
                buttonsTriple.SetActive(isActive);
                break;
        }
    }

    private enum ChoiceType
    {
        DOUBLE,
        TRIPLE
    }
}