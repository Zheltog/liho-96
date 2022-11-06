using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Boss
{
    public class PhaseConfigurator : MonoBehaviour
    {
        public TextBoxController text;
        public GameObject enemyBenchLeft;
        public GameObject enemyBenchRight;
        public Light enemiesLight;
        public float minLight = 0.1f;

        private MainController _mainController;

        private void Start()
        {
            _mainController = GetComponent<MainController>();
        }

        public void NewPhase()
        {
            var phase = StateHolder.NextPhase();
            text.NewText(phase.StartText);
            _mainController.ApplyEffect(phase.Effect);
            ApplyModifiers(phase.Modifiers);
            DisableAllEnemies();
            SetEnemies(phase.Enemies);
        }

        private void DisableAllEnemies()
        {
            enemyBenchLeft.SetActive(false);
            enemyBenchRight.SetActive(false);
        }
        
        private void SetEnemies(List<EnemyType> enemies)
        {
            if (enemies.Contains(EnemyType.BenchLeft))
            {
                enemyBenchLeft.SetActive(true);
            }
            if (enemies.Contains(EnemyType.BenchRight))
            {
                enemyBenchRight.SetActive(true);
            }
        }

        private void ResetModifiers()
        {
            enemiesLight.intensity = 1f;
        }

        private void ApplyModifiers(List<Modifier> modifiers)
        {
            if (modifiers.Contains(Modifier.Dark))
            {
                enemiesLight.intensity = minLight;
            }
        }
    }
}