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
            text.NewText(item.UseText);
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
            // TODO: открыть, но дать возможность вернуться
            if (!itemsChoicesController.ItemsChoiceAvailable()) return;
            
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
            _enemiesController.DisableAllEnemies();   // TODO: вынести куда-то из фазоконфига?
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            text.NewText(comment);
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
            text.NewText(" ");
            grayPanel.SetActive(false);
            courier.StopRest();
        }

        public enum FightState
        {
            Initializing, NewPhase, Attack, ActionChoice, ItemChoosing, ItemChosen, GameOver
        }

        // TODO: удалить, нужен для тестирования
        private void InitHolder()
        {
            var items = new List<Item>();
            items.Add(new Item("Вейп", "<Курьер оформляет плотнейшего пыха и восстанавливает 10 очков здоровья>",
                "флаг", new Effect(EffectType.Heal, 10f)));
            items.Add(new Item("Журнал \"Playboy\"",
                "<Курьер пристально всматривается в обложку журнала. Вспомнив, за что должно сражать настоящему мужчине, он издаёт свирепый рык, нанося противникам 10 очков урона>",
                "флаг", new Effect(EffectType.Damage, 10f)));
            items.Add(new Item("Папка со сценарием фильма \"Довод\"",
                "<Пробежав сценарий глазами, курьер осознаёт гениальность Нолана и теперь умеет влиять на ход времени. +10 секунд к заводу жиги!>",
                "флаг", new Effect(EffectType.Timer, 10f)));

            var phases = new List<Phase>();
            var enemies1 = new List<EnemyType>();
            var enemies2 = new List<EnemyType>();
            enemies2.Add(EnemyType.BenchLeft);
            enemies2.Add(EnemyType.BenchRight);
            var enemies3 = new List<EnemyType>();
            enemies3.Add(EnemyType.BenchLeft);
            enemies3.Add(EnemyType.BenchRight);
            enemies3.Add(EnemyType.RunningLeft);
            enemies3.Add(EnemyType.RunningRight);
            var modifiers2 = new List<Modifier>();
            modifiers2.Add(Modifier.Dark);
            var modifiers3 = new List<Modifier>();
            modifiers3.Add(Modifier.Dark);
            modifiers3.Add(Modifier.Shake);
            phases.Add(new Phase(
                PhaseType.Shooting,
                "Фаза1. Ну всё пиздец...",
                null,
                enemies3,
                new List<Modifier>()
            ));
            phases.Add(new Phase(
                PhaseType.Shooting,
                "Фаза2. Ахуеть не встать (а если встать, то ахуеть)... Игрок немного подлечился, но темно пиздец",
                new Effect(EffectType.Heal, 10f),
                enemies2,
                modifiers2
            ));
            phases.Add(new Phase(
                PhaseType.Shooting,
                "Фаза3. Ебать копать темно пизды ещё и трясёт как блять",
                new Effect(EffectType.Heal, 10f),
                enemies3,
                modifiers3
            ));
            
            StateHolder.Init(items, phases);
        }
    }
}