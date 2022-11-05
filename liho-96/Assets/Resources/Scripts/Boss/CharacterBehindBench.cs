using UnityEngine;

namespace Boss
{
    public abstract class CharacterBehindBench : MonoBehaviour
    {
        public float hp = 50;
        public float damage;
        public HealthBar hpBar;

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
            
            var hpDamage = damageTaken;

            hp -= damageTaken;

            if (hp <= 0)
            {
                Die();
            }

            hpBar.Damage(hpDamage);
            OnDamage();
        }

        protected abstract bool ShouldTakeDamage();

        protected abstract void Die();
        protected abstract void OnDamage();
    }
}