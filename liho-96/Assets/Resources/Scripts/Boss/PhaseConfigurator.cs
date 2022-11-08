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
        public GameObject enemyRunningLeft;
        public GameObject enemyRunningRight;
        public Courier courier;
        public Light sceneLight;
        public float minLight = 0.1f;
        public float maxLight = 1f;

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
            ResetModifiers();
            ApplyModifiers(phase.Modifiers);
            DisableAllEnemies();
            SetEnemies(phase.Enemies);
        }

        public void DisableAllEnemies()
        {
            enemyBenchLeft.SetActive(false);
            enemyBenchRight.SetActive(false);
            enemyRunningLeft.SetActive(false);
            enemyRunningRight.SetActive(false);
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
            if (enemies.Contains(EnemyType.RunningLeft))
            {
                enemyRunningLeft.SetActive(true);
            }
            if (enemies.Contains(EnemyType.RunningRight))
            {
                enemyRunningRight.SetActive(true);
            }
        }

        private void ResetModifiers()
        {
            sceneLight.intensity = maxLight;
            courier.CameraShake(false);
        }

        private void ApplyModifiers(List<Modifier> modifiers)
        {
            if (modifiers.Contains(Modifier.Dark))
            {
                sceneLight.intensity = minLight;
            }

            if (modifiers.Contains(Modifier.Shake))
            {
                courier.CameraShake(true);
            }
        }
    }
}