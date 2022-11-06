using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class HealthBar : MonoBehaviour
    {
        public float maxHealth = 100f;
        public string pulseAnimationName = "";

        private float _currentHealth;
        private Slider _bar;
        private Animator _animator;

        private void Start()
        {
            _bar = GetComponent<Slider>();
            _animator = GetComponent<Animator>();
            _currentHealth = maxHealth;
        }

        public void AddHp(float hp, bool pulse)
        {
            _currentHealth += hp;
            _bar.value = _currentHealth / maxHealth;

            if (pulse)
            {
                Pulse();
            }
        }
        
        private void Pulse()
        {
            _animator.Play(pulseAnimationName);
        }
    }
}