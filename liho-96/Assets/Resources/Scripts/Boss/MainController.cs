using System;
using Common;
using TMPro;
using UnityEngine;

namespace Boss
{
    public class MainController : MonoBehaviour
    {
        public float fightSeconds = 120;
        public float activePhaseSeconds = 10;
        public Courier courier;
        public GameObject actionsButtons;
        public TimerController timer;
        public TextBoxController text;
        public ItemsChoicesController itemsChoicesController;
        
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
            if (!itemsChoicesController.ItemsChoiceAvailable()) return;
            
            actionsButtons.SetActive(false);
            itemsChoicesController.NewChoices();
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

        private void UpdateTimeRemainingBeforeNextRest()
        {
            if (_isRest)
            {
                return;
            }

            if (_timeRemainingBeforeNextRest > 0)
            {
                _timeRemainingBeforeNextRest -= Time.deltaTime;
                return;
            }

            Rest();
            _timeRemainingBeforeNextRest = activePhaseSeconds;
        }

        private void UpdateTimeRemainingBeforeEnd()
        {
            if (_timeRemainingBeforeEnd > 0)
            {
                _timeRemainingBeforeEnd -= Time.deltaTime;
                timer.SetTimeRemaining(_timeRemainingBeforeEnd);
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

        private void ReturnToActivePhase()
        {
            _isRest = false;
            text.NewText(" ");
            courier.ReturnToActivePhase();
        }
    }
}