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
        
        private float _timeRemainingBeforeNextRest;
        private PhaseConfigurator _phaseConfig;
        private EnemiesController _enemiesController;
        private ScenesController _scenes;
        private CursorController _cursor;
        private FightState _currentFightState = FightState.Initializing;

        private void Start()
        {
            _timeRemainingBeforeNextRest = phaseSeconds;
            _phaseConfig = GetComponent<PhaseConfigurator>();
            _enemiesController = GetComponent<EnemiesController>();
            _scenes = GetComponent<ScenesController>();
            _cursor = GetComponent<CursorController>();
            StartCoroutine(NextPhaseWithDelay());
        }

        private void Update()
        {
            if (_currentFightState == FightState.Attack)
            {
                UpdateTimeRemainingBeforeNextRest();
                return;
            }

            // TODO: очень костыль
            if (Input.GetMouseButtonDown(0) && _cursor.IsCursorInsideCrtLines)
            {
                FinishPrintingOrUpdateRoundState();
            }
        }
        
        public void ChooseItem(Item item)
        {
            _currentFightState = FightState.ItemChosen;
            backButton.SetActive(false);
            text.NewText(item.UseText);
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
            if (_currentFightState == FightState.Initializing)
            {
                return;
            }
            
            if (text.IsPrinting)
            {
                text.FinishPrinting();
                return;
            }

            switch (_currentFightState)
            {
                case FightState.NewPhase:
                    NextAttack();
                    break;
                case FightState.ItemChosen:
                    NextPhase();
                    break;
                case FightState.GameOver:
                    _scenes.Exit();
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
            _currentFightState = FightState.ItemChoosing;
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
            text.NewText(" ");
            itemsChoicesController.DisableButtons();
            backButton.SetActive(false);
            actionsButtons.SetActive(true);
            _currentFightState = FightState.ActionChoice;
        }

        public void HealthBarEmpty(HealthBarType type)
        {
            switch (type)
            {
                case HealthBarType.Courier:
                    GameOver(CommentsHolder.CourierDied);
                    break;
                case HealthBarType.Enemies:
                    if (_currentFightState == FightState.ItemChosen)
                    {
                        _currentFightState = FightState.LastItemChosen;
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
            _currentFightState = FightState.GameOver;
            courier.Rest();
            _enemiesController.DisableAllEnemies();
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            text.NewText(comment);
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
            _currentFightState = FightState.ActionChoice;
            courier.Rest();
            actionsButtons.SetActive(true);
            grayPanel.SetActive(true);
            _enemiesController.DisableAllEnemies();
        }

        private IEnumerator NextPhaseWithDelay()
        {
            yield return new WaitForSeconds(1);
            NextPhase();
        }

        private void NextPhase()
        {
            _currentFightState = FightState.NewPhase;
            _phaseConfig.NewPhase();
        }
        
        private void NextAttack()
        {
            _currentFightState = FightState.Attack;
            text.NewText(" ");
            grayPanel.SetActive(false);
            courier.StopRest();
        }

        private enum FightState
        {
            Initializing, NewPhase, Attack, ActionChoice, ItemChoosing, ItemChosen, LastItemChosen, GameOver
        }
    }
}