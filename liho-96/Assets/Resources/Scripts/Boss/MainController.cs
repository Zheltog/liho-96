using Common;
using UnityEngine;

namespace Boss
{
    public class MainController : MonoBehaviour
    {
        public float fightSeconds = 30;
        public float activePhaseSeconds = 10;
        public Courier courier;
        public GameObject dynamicStuff;
        public GameObject gameOverImage;
        public GameObject actionsButtons;
        public AudioController player;
        public TimerController timer;
        public TextBoxController text;
        public ItemsChoicesController itemsChoicesController;
        
        private float _timeRemainingBeforeEnd;
        private float _timeRemainingBeforeNextRest;
        private RoundState _currentRoundState = RoundState.Attack;

        private void Start()
        {
            _timeRemainingBeforeEnd = fightSeconds;
            _timeRemainingBeforeNextRest = activePhaseSeconds;
        }

        private void Update()
        {
            if (_currentRoundState == RoundState.GameOver)
            {
                return;
            }
            
            UpdateTimeRemainingBeforeNextRest();
            UpdateTimeRemainingBeforeEnd();
        }
        
        public void ChooseItem(Item item)
        {
            text.NewText(item.UseText);
            itemsChoicesController.DisableButtons();
        }

        public void FinishPrintingOrUpdateRoundState()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            else
            {
                switch (_currentRoundState)
                {
                    case RoundState.ItemChoice:
                        ReturnToActivePhase();
                        break;
                    case RoundState.GameOver:
                        Application.Quit();
                        break;
                }
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
            
            _currentRoundState = RoundState.ItemChoice;
            actionsButtons.SetActive(false);
            itemsChoicesController.NewChoices();
        }

        public void Surrender()
        {
            GameOver("Курьер сдался. Pathetic.");
        }
        
        public void GameOver(string comment)
        {
            _currentRoundState = RoundState.GameOver;
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            text.NewText(comment);
            player.NewMusic("game_over");
        }

        private void UpdateTimeRemainingBeforeNextRest()
        {
            if (_currentRoundState != RoundState.Attack)
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
            if (_timeRemainingBeforeEnd <= 0)
            {
                GameOver("Коммунисты завели жигу и уехали в рассвет. С ЛИХО...");
                return;
            }
            
            _timeRemainingBeforeEnd -= Time.deltaTime;
            timer.SetTimeRemaining(_timeRemainingBeforeEnd);
        }

        private void Rest()
        {
            _currentRoundState = RoundState.ActionChoice;
            courier.Rest();
            actionsButtons.SetActive(true);
        }

        private void ReturnToActivePhase()
        {
            _currentRoundState = RoundState.Attack;
            text.NewText(" ");
            courier.ReturnToActivePhase();
        }

        private enum RoundState
        {
            Attack, ActionChoice, ItemChoice, GameOver
        }
    }
}