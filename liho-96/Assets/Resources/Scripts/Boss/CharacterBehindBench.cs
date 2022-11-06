using UnityEngine;

namespace Boss
{
    public abstract class CharacterBehindBench : MonoBehaviour
    {
        public float hp = 50;
        public float damage;
        public HealthBar hpBar;

        public void AddHp(float newHp, bool pulseBar)
        {
            hp += newHp;
            hpBar.AddHp(newHp, pulseBar);
        }
        
        protected void Shoot(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit)) return;
            var enemy = hit.collider.gameObject.GetComponent<CharacterBehindBench>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
        
        protected void Hit(float damageTaken)
        {
            if (!ShouldTakeDamage())
            {
                return;
            }

            hp -= damageTaken;

            if (hp <= 0)
            {
                Die();
            }

            hpBar.AddHp(-1 * damageTaken, false);
            OnDamage();
        }

        protected abstract bool ShouldTakeDamage();

        protected abstract void Die();
        protected abstract void OnDamage();
    }
}