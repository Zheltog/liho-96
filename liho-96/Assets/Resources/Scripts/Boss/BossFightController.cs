using System;
using TMPro;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    public float fightSeconds = 120;
    public float activePhaseSeconds = 10;
    public TextMeshProUGUI timer;
    public Courier courier;
    public BossFightItemsChoicesController itemsChoicesController;
    public GameObject actionsButtons;

    public TextBoxController text;

    private float _timeRemainingBeforeEnd;
    private float _timeRemainingBeforeNextRest;
    private bool _isRest;

    private void Start()
    {
        _timeRemainingBeforeEnd = fightSeconds;
        _timeRemainingBeforeNextRest = activePhaseSeconds;
    }

    private void Update()
    {
        UpdateTimeRemainingBeforeNextRest();
        UpdateTimeRemainingBeforeEnd();
    }

    public void ReturnToActivePhase()
    {
        _isRest = false;
        text.NewText(" ");
        courier.ReturnToActivePhase();
    }

    private void UpdateTimeRemainingBeforeNextRest()
    {
        if (_isRest)
        {
            return;
        }
        
        if (_timeRemainingBeforeNextRest > 0)
        {
            _timeRemainingBeforeNextRest -= Time.deltaTime;
        }
        else
        {
            Rest();
            _timeRemainingBeforeNextRest = activePhaseSeconds;
        }
    }

    private void UpdateTimeRemainingBeforeEnd()
    {
        if (_timeRemainingBeforeEnd > 0)
        {
            _timeRemainingBeforeEnd -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(_timeRemainingBeforeEnd / 60);
            float seconds = Mathf.FloorToInt(_timeRemainingBeforeEnd % 60);
            timer.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.Log("TIME RAN OUT!");
        }
    }

    private void Rest()
    {
        _isRest = true;
        courier.Rest();
        actionsButtons.SetActive(true);
    }

    public void ChooseItem(Item item)
    {
        text.NewText(item.UseText);
        itemsChoicesController.DisableButtons();
    }

    public void FinishPrintingOrStopRest()
    {
        if (text.IsPrinting)
        {
            text.FinishPrinting();
        }
        else
        {
            ReturnToActivePhase();
        }
    }

    public void Attack()
    {
        actionsButtons.SetActive(false);
        ReturnToActivePhase();
    }

    public void Inventory()
    {
        if (itemsChoicesController.ItemsChoiceAvailable())
        {
            actionsButtons.SetActive(false);
            itemsChoicesController.NewChoices();
        }
    }

    public void Surrender()
    {
        try
        {
            throw new Exception("");
        }
        catch (Exception e)
        {
            actionsButtons.SetActive(false);
            text.NewText(e.StackTrace);
        }
    }
}