using System.Collections;
using UnityEngine;

namespace Boss
{
    public abstract class Enemy : MonoBehaviour
    {
        public GameObject shotLight;
        public HealthBar hpBar;
        public Vector3 targetPoint = new Vector3(0f, 5f, -10f);
        public float hp = 50;
        public float damage;
        public float shotLightSeconds = 0.5f;
        public float secondsBeforeNextShooting = 2f;
        
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
            var fromPosition = transform.position;
            var direction = targetPoint - fromPosition;
            var ray = new Ray(transform.position, direction);
            if (!Physics.Raycast(ray, out var hit)) return;
            var courier = hit.collider.gameObject.GetComponent<Courier>();
            if (courier != null)
            {
                courier.Hit(damage);
            }

            StartCoroutine(ShowShotLight());
        }

        private IEnumerator ShowShotLight()
        {
            shotLight.SetActive(true);
            yield return new WaitForSeconds(shotLightSeconds);
            shotLight.SetActive(false);
        }

        protected abstract void OnDamage();
    }
}