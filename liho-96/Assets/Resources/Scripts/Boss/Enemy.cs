using UnityEngine;

namespace Boss
{
    public abstract class Enemy : MonoBehaviour
    {
        public HealthBar hpBar;
        public Vector3 targetPoint = new Vector3(0f, 5f, -10f);
        public float hp = 50;
        public float damage;
        public float secondsBeforeNextShot = 2f;
        
        private float _currentTime;
        
        public void Hit(float damageTaken)
        {
            hp -= damageTaken;

            if (hp <= 0)
            {
                gameObject.SetActive(false);
            }

            hpBar.AddHp(-1 * damageTaken, false);

            OnDamage();
        }
        
        protected void Shoot()
        {
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShot)) return;
            _currentTime -= secondsBeforeNextShot;
            
            var fromPosition = transform.position;
            var direction = targetPoint - fromPosition;
            var ray = new Ray(transform.position, direction);
            if (!Physics.Raycast(ray, out var hit)) return;
            var courier = hit.collider.gameObject.GetComponent<Courier>();
            if (courier != null)
            {
                courier.Hit(damage);
            }
        }

        protected abstract void OnDamage();
    }
}