using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class HealthBar : MonoBehaviour
    {
        public float maxHealth = 100f;

        private float _currentHealth;
        private Slider _bar;

        private void Start()
        {
            _bar = GetComponent<Slider>();
            _currentHealth = maxHealth;
        }

        public void AddHp(float hp)
        {
            _currentHealth += hp;
            _bar.value = _currentHealth / maxHealth;
        }
    }
}