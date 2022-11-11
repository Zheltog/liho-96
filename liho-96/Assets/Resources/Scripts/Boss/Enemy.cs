using System.Collections;
using Common;
using UnityEngine;

namespace Boss
{
    public abstract class Enemy : MonoBehaviour
    {
        public AudioController player;
        public GameObject shotBang;
        public HealthBar hpBar;
        public Vector3 targetPoint = new Vector3(0f, 5f, -10f);
        public float hp = 50;
        public float damage = 5;
        public float shotBangSeconds = 0.5f;
        public float secondsBeforeNextShooting = 2f;
        public float redOnHitSeconds = 0.2f;

        private SpriteRenderer _sprite;

        protected void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        public abstract void Reset();

        public void Hit(float damageTaken)
        {
            hp -= damageTaken;

            if (hp <= 0)
            {
                gameObject.SetActive(false);
            }

            hpBar.AddHp(-1 * damageTaken, false);

            StartCoroutine(RedOnHit());
            OnDamage();
        }
        
        protected void Shoot()
        {
            var fromPosition = transform.position;
            var direction = targetPoint - fromPosition;
            var ray = new Ray(fromPosition, direction);
            if (!Physics.Raycast(ray, out var hit)) return;
            var courier = hit.collider.gameObject.GetComponent<Courier>();
            if (courier == null) return;
            courier.Hit(damage);
            player.NewSound("shot");
            StartCoroutine(ShowShotBang());
        }

        private IEnumerator RedOnHit()
        {
            _sprite.color = Color.red;
            yield return new WaitForSeconds(redOnHitSeconds);
            _sprite.color = Color.white;
        }

        private IEnumerator ShowShotBang()
        {
            shotBang.SetActive(true);
            yield return new WaitForSeconds(shotBangSeconds);
            shotBang.SetActive(false);
        }

        protected abstract void OnDamage();
    }
}