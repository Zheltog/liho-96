using UnityEngine;
using UnityEngine.UI;

public class EnemiesHealthBar : MonoBehaviour
{
    public float maxHealth = 100f;

    private float _currentHealth;
    private Slider _bar;

    private void Start()
    {
        _bar = GetComponent<Slider>();
        _currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;
        _bar.value = _currentHealth / maxHealth;
    }
}