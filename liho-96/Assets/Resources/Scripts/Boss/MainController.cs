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
        public TimerController timer;
        public AudioController player;
        public TextBoxController text;
        public ItemsChoicesController itemsChoicesController;
        public RoundState CurrentRoundState { get; private set; }
        
        private float _timeRemainingBeforeNextRest;
        private PhaseConfigurator _phaseConfig;

        private void Start()
        {
            InitHolder();
            _timeRemainingBeforeNextRest = phaseSeconds;
            _phaseConfig = GetComponent<PhaseConfigurator>();
            NextPhase();
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
            ApplyEffect(item.Effect);
            CurrentRoundState = RoundState.ItemChosen;
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
            if (text.IsPrinting)
            {
                text.FinishPrinting();
            }
            else
            {
                switch (CurrentRoundState)
                {
                    case RoundState.NewPhase:
                        NextAttack();
                        break;
                    case RoundState.ItemChosen:
                        NextPhase();
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
            NextPhase();
        }

        public void Inventory()
        {
            if (!itemsChoicesController.ItemsChoiceAvailable()) return;
            
            CurrentRoundState = RoundState.ItemChoosing;
            actionsButtons.SetActive(false);
            itemsChoicesController.NewChoices();
        }

        public void Surrender()
        {
            GameOver("Курьер сдался. Pathetic.");
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
            CurrentRoundState = RoundState.GameOver;
            dynamicStuff.SetActive(false);
            gameOverImage.SetActive(true);
            text.NewText(comment);
            player.NewMusic("");
            player.NewSound("game_over");
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
            _timeRemainingBeforeNextRest = phaseSeconds;
        }

        private void Rest()
        {
            CurrentRoundState = RoundState.ActionChoice;
            courier.Rest();
            actionsButtons.SetActive(true);
            grayPanel.SetActive(true);
        }

        private void NextPhase()
        {
            CurrentRoundState = RoundState.NewPhase;
            _phaseConfig.NewPhase();
        }
        
        private void NextAttack()
        {
            CurrentRoundState = RoundState.Attack;
            text.NewText(" ");
            grayPanel.SetActive(false);
        }

        public enum RoundState
        {
            NewPhase, Attack, ActionChoice, ItemChoosing, ItemChosen, GameOver
        }
        
        //
        
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
            enemies1.Add(EnemyType.BenchLeft);
            var enemies2 = new List<EnemyType>();
            enemies2.Add(EnemyType.BenchLeft);
            enemies2.Add(EnemyType.BenchRight);
            var modifiers = new List<Modifier>();
            modifiers.Add(Modifier.Dark);
            phases.Add(new Phase(
                PhaseType.Shooting,
                "Фаза1. Ну всё пиздец...",
                null,
                enemies1,
                new List<Modifier>()
            ));
            phases.Add(new Phase(
                PhaseType.Shooting,
                "Фаза2. Ахуеть не встать (а если встать, то ахуеть)... Игрок немного подлечился, но темно пиздец",
                new Effect(EffectType.Heal, 10f),
                enemies2,
                modifiers
            ));
            
            StateHolder.Init(items, phases);
        }
    }
}