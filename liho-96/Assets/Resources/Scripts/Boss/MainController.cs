using Common;
using UnityEngine;

namespace Boss
{
    public class MainController : MonoBehaviour
    {
        public float activePhaseSeconds = 10;
        public Courier courier;
        public GameObject grayPanel;
        public GameObject dynamicStuff;
        public GameObject gameOverImage;
        public GameObject actionsButtons;
        public TimerController timer;
        public AudioController player;
        public TextBoxController text;
        public ItemsChoicesController itemsChoicesController;
        public RoundState CurrentRoundState { get; private set; }
        
        private float _timeRemainingBeforeNextRest;

        private void Start()
        {
            CurrentRoundState = RoundState.Attack;
            _timeRemainingBeforeNextRest = activePhaseSeconds;
        }

        private void Update()
        {
            if (CurrentRoundState == RoundState.GameOver)
            {
                return;
            }
            
            UpdateTimeRemainingBeforeNextRest();
        }
        
        public void ChooseItem(Item item)
        {
            text.NewText(item.UseText);
            itemsChoicesController.DisableButtons();
            
            var effectValue = item.Effect.Value;
            switch (item.Effect.Type)
            {
                case EffectType.Damage:
                    break;
                case EffectType.Heal:
                    courier.AddHp(effectValue);
                    break;
                case EffectType.Timer:
                    timer.AddTime(effectValue);
                    break;
            }
        }

        public void FinishPrintingOrUpdateRoundState()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            else
            {
                switch (CurrentRoundState)
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
            
            CurrentRoundState = RoundState.ItemChoice;
            actionsButtons.SetActive(false);
            itemsChoicesController.NewChoices();
        }

        public void Surrender()
        {
            GameOver("Курьер сдался. Pathetic.");
        }
        
        public void GameOver(string comment)
        {
            CurrentRoundState = RoundState.GameOver;
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            text.NewText(comment);
            player.NewMusic("game_over");
        }

        private void UpdateTimeRemainingBeforeNextRest()
        {
            if (CurrentRoundState != RoundState.Attack)
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

        private void Rest()
        {
            CurrentRoundState = RoundState.ActionChoice;
            courier.Rest();
            actionsButtons.SetActive(true);
            grayPanel.SetActive(true);
        }

        private void ReturnToActivePhase()
        {
            CurrentRoundState = RoundState.Attack;
            text.NewText(" ");
            grayPanel.SetActive(false);
        }

        public enum RoundState
        {
            Attack, ActionChoice, ItemChoice, GameOver
        }
    }
}