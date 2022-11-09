using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Boss
{
    public class PhaseConfigurator : MonoBehaviour
    {
        public TextBoxController text;
        public Courier courier;
        public Light sceneLight;
        public float minLight = 0.1f;
        public float maxLight = 1f;

        private MainController _mainController;
        private EnemiesController _enemies;

        private void Start()
        {
            _mainController = GetComponent<MainController>();
            _enemies = GetComponent<EnemiesController>();
        }

        public void NewPhase()
        {
            var phase = StateHolder.NextPhase();
            text.NewText(phase.StartText);
            _mainController.ApplyEffect(phase.Effect);
            ResetModifiers();
            ApplyModifiers(phase.Modifiers);
            _enemies.SetEnemies(phase.Enemies);
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