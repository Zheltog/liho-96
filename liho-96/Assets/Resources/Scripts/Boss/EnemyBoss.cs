using UnityEngine;

namespace Boss
{
    public class EnemyBoss: Enemy
    {
        public float secondsBeforeNextBullet = 0.2f;
        public float magazineSize = 10;
        
        private float _currentTime;
        private bool _isShooting;
        private int _currentBulletNumber = 1;
        
        private void Update()
        {
            if (_isShooting)
            {
                _currentTime += Time.deltaTime;
                if (!(_currentTime >= secondsBeforeNextBullet)) return;
                _currentTime -= secondsBeforeNextBullet;
                ShootNextBullet();
                return;
            }
            
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShooting)) return;
            _currentTime -= secondsBeforeNextShooting;
            _isShooting = true;
        }

        private void ShootNextBullet()
        {
            if (_currentBulletNumber > magazineSize)
            {
                _currentBulletNumber = 1;
                _isShooting = false;
                return;
            }

            Shoot();
            StartCoroutine(ShowShotBang());
            _currentBulletNumber++;
        }

        protected override void OnDamage()
        {
            // TODO
            Debug.Log("A!");
        }
    }
}