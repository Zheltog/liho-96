using System.Collections.Generic;
using UnityEngine;

public class ButtonsBundle : MonoBehaviour
{

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    private GameController _gameController;
    private List<Choice> _currentChoices;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void Activate(List<Choice> choices)
    {
        _currentChoices = choices;
        
        var choice1 = choices[0];
        var choice2 = choices[1];
        Choice choice3 = null;

        if (choices.Count == 3)
        {
            choice3 = choices[2];
        }
        
        SetButtonsActive(
            ChoiceAvailabilityCalculator.IsAvailable(choice1),
            ChoiceAvailabilityCalculator.IsAvailable(choice2),
            ChoiceAvailabilityCalculator.IsAvailable(choice3)
        );
    }
    
    public void ChooseFirst()
    {
        _gameController.Transition(_currentChoices[0].Transition);
        SetButtonsActive(false, false, false);
    }
    
    public void ChooseSecond()
    {
        _gameController.Transition(_currentChoices[1].Transition);
        SetButtonsActive(false, false, false);
    }

    public void ChooseThird()
    {
        _gameController.Transition(_currentChoices[2].Transition);
        SetButtonsActive(false, false, false);
    }
    
    private void SetButtonsActive(bool is1Active, bool is2Active, bool is3Active)
    {
        button1.SetActive(is1Active);
        button2.SetActive(is2Active);
        if (button3 != null) button3.SetActive(is3Active);
    }
}