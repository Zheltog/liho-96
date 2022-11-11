using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            CurrentFightState = FightState.Initializing;
            InitHolder();
            _timeRemainingBeforeNextRest = phaseSeconds;
            _phaseConfig = GetComponent<PhaseConfigurator>();
            _enemiesController = GetComponent<EnemiesController>();
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
            backButton.SetActive(false);
            NewText(item.UseText);
            itemsChoicesController.DisableButtons();
            ApplyEffect(item.Effect);
            CurrentFightState = FightState.ItemChosen;
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
            itemsChoicesController.NewChoices();
        }

        public void Surrender()
        {
            GameOver("Курьер сдался. Pathetic.");
        }

        public void Back()
        {
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
                    GameOver("Райан Гослинг умер...");
                    break;
                case HealthBarType.Enemies:
                    GameOver("Райан Гослинг убил всех и стал плохим, а Райан Гослинг не может быть плохим... пиздец...");
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
            _enemiesController.DisableAllEnemies();   // TODO: вынести куда-то из фазоконфига?
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
            Initializing, NewPhase, Attack, ActionChoice, ItemChoosing, ItemChosen, GameOver
        }

        // TODO: удалить, инициализация холдера будет в конце фрейм-сцены
        private void InitHolder()
        {
            var flags = new List<string>();
            flags.Add("Vodka");
            flags.Add("Gum");
            flags.Add("Snickers");
            flags.Add("GirlPhoneNumber");
            flags.Add("Wires");
            flags.Add("Dumbbell");
            flags.Add("Grenade");
            flags.Add("Molotov");
            flags.Add("Awl");
            flags.Add("PartyPhoneNumber");
            // flags.Add("Rag");
            // flags.Add("Coins");
            StateHolder.Init(flags);
        }
    }
}