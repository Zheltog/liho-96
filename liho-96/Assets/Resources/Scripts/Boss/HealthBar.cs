using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class HealthBar : MonoBehaviour
    {
        public MainController mainController;
        public HealthBarType type;
        public float maxHealth = 100f;

        private float _currentHealth;
        private Slider _bar;
        private Animator _animator;

        private void Start()
        {
            _bar = GetComponent<Slider>();
            _animator = GetComponent<Animator>();
            _currentHealth = maxHealth;
        }

        public void AddHp(float hp, bool shouldPulse)
        {
            _animator.Rebind();
            _animator.Update(0f);
            
            _currentHealth += hp;
            _bar.value = _currentHealth / maxHealth;

            if (shouldPulse)
            {
                Pulse();
            }
            
            if (_currentHealth <= 0)
            {
                Empty();
            }

            if (_currentHealth > maxHealth)
            {
                _currentHealth = maxHealth;
            }
        }

        private void Empty()
        {
            mainController.HealthBarEmpty(type);
        }
        
        private void Pulse()
        {
            _animator.Play(type + "HealthBarPulse");
        }
    }

    public enum HealthBarType
    {
        Courier, Enemies
    }
}