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
        SetActive(true);
    }
    
    public void ChooseFirst()
    {
        _gameController.Transition(_currentChoices[0].Transition);
        SetActive(false);
    }
    
    public void ChooseSecond()
    {
        _gameController.Transition(_currentChoices[1].Transition);
        SetActive(false);
    }

    public void ChooseThird()
    {
        _gameController.Transition(_currentChoices[2].Transition);
        SetActive(false);
    }
    
    private void SetActive(bool isActive)
    {
        button1.SetActive(isActive);
        button2.SetActive(isActive);
        if (button3 != null) button3.SetActive(isActive);
    }
}