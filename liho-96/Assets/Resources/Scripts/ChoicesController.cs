using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesController : MonoBehaviour
{
    public ButtonsBundle buttonsDouble;
    public ButtonsBundle buttonsTriple;

    public void NewChoices(List<Choice> choices)
    {
        switch (choices.Count)
        {
            case 2:
                buttonsDouble.Activate(choices);
                break;
            case 3:
                buttonsTriple.Activate(choices);
                break;
            default:
                throw new Exception("should be 2 or 3 choices");
        }
    }
}