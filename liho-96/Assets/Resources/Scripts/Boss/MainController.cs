using System.Collections;
using Common;
using UnityEngine;

namespace Boss
{
    public class MainController : MonoBehaviour
    {
        public float phaseSeconds = 10;
        public Courier courier;
        public HealthBar enemiesHealthBar;
        public GameObject grayPanel;
        public GameObject dynamicStuff;
        public GameObject gameOverImage;
        public GameObject actionsButtons;
        public GameObject backButton;
        public TimerController timer;
        public AudioController player;
        public TextBoxController text;
        public ItemsChoicesController itemsChoicesController;
        public FightState CurrentFightState { get; private set; }
        
        private float _timeRemainingBeforeNextRest;
        private PhaseConfigurator _phaseConfig;
        private EnemiesController _enemiesController;
        private ScenesController _scenes;

        private void Start()
        {
            CurrentFightState = FightState.Initializing;
            _timeRemainingBeforeNextRest = phaseSeconds;
            _phaseConfig = GetComponent<PhaseConfigurator>();
            _enemiesController = GetComponent<EnemiesController>();
            _scenes = GetComponent<ScenesController>();
            StartCoroutine(NextPhaseWithDelay());
        }

        private void Update()
        {
            if (CurrentFightState == FightState.Attack)
            {
                UpdateTimeRemainingBeforeNextRest();
            }
        }
        
        public void ChooseItem(Item item)
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            CurrentFightState = FightState.ItemChosen;
            backButton.SetActive(false);
            NewText(item.UseText);
            itemsChoicesController.DisableButtons();
            ApplyEffect(item.Effect);
        }

        public void ApplyEffect(Effect effect)
        {
            if (effect == null)
            {
                return;
            }
            
            var effectValue = effect.Value;
            switch (effect.Type)
            {
                case EffectType.Damage:
                    enemiesHealthBar.AddHp(-1 * effectValue, true);
                    break;
                case EffectType.Heal:
                    courier.hpBar.AddHp(effectValue, true);
                    break;
                case EffectType.Timer:
                    timer.AddTime(effectValue);
                    break;
            }
        }

        public void FinishPrintingOrUpdateRoundState()
        {
            if (CurrentFightState == FightState.Initializing)
            {
                return; // TODO починить как-нибудь, убрать Initializing
            }
            
            if (text.IsPrinting)
            {
                text.FinishPrinting();
                return;
            }

            switch (CurrentFightState)
            {
                case FightState.NewPhase:
                    NextAttack();
                    break;
                case FightState.ItemChosen:
                    NextPhase();
                    break;
                case FightState.GameOver:
                    Application.Quit();
                    break;
                case FightState.LastItemChosen:
                    Win();
                    break;
            }
        }

        public void Attack()
        {
            actionsButtons.SetActive(false);
            NextPhase();
        }

        public void Inventory()
        {
            CurrentFightState = FightState.ItemChoosing;
            actionsButtons.SetActive(false);
            backButton.SetActive(true);
            if (!itemsChoicesController.ItemsChoiceAvailable())
            {
                text.NewText(CommentsHolder.NoItems);
            }
            else
            {
                itemsChoicesController.NewChoices();
            }
        }

        public void Surrender()
        {
            GameOver(CommentsHolder.CourierSurrendered);
        }

        public void ToActionChoice()
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            text.NewText(" ");
            itemsChoicesController.DisableButtons();
            backButton.SetActive(false);
            actionsButtons.SetActive(true);
            CurrentFightState = FightState.ActionChoice;
        }

        public void HealthBarEmpty(HealthBarType type)
        {
            switch (type)
            {
                case HealthBarType.Courier:
                    GameOver(CommentsHolder.CourierDied);
                    break;
                case HealthBarType.Enemies:
                    if (CurrentFightState == FightState.ItemChosen)
                    {
                        CurrentFightState = FightState.LastItemChosen;
                    }
                    else
                    {
                        Win();
                    }
                    break;
            }
        }
        
        public void GameOver(string comment)
        {
            CurrentFightState = FightState.GameOver;
            courier.Rest();
            _enemiesController.DisableAllEnemies();
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            NewText(comment);
            player.NewMusic("");
            player.NewSound("game_over");
        }

        public void Win()
        {
            _scenes.LoadPreviousScene();
        }

        private void UpdateTimeRemainingBeforeNextRest()
        {
            if (_timeRemainingBeforeNextRest > 0)
            {
                _timeRemainingBeforeNextRest -= Time.deltaTime;
                return;
            }

            Rest();
            _timeRemainingBeforeNextRest = phaseSeconds;
        }

        private void Rest()
        {
            CurrentFightState = FightState.ActionChoice;
            courier.Rest();
            actionsButtons.SetActive(true);
            grayPanel.SetActive(true);
            _enemiesController.DisableAllEnemies();
        }

        // TODO: починить и убрать?
        private IEnumerator NextPhaseWithDelay()
        {
            yield return new WaitForSeconds(1);
            NextPhase();
        }

        private void NextPhase()
        {
            CurrentFightState = FightState.NewPhase;
            _phaseConfig.NewPhase();
        }
        
        private void NextAttack()
        {
            CurrentFightState = FightState.Attack;
            NewText(" ");
            grayPanel.SetActive(false);
            courier.StopRest();
        }

        private void NewText(string newText)
        {
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            text.NewText(newText);
        }

        public enum FightState
        {
            Initializing, NewPhase, Attack, ActionChoice, ItemChoosing, ItemChosen, LastItemChosen, GameOver
        }
    }
}