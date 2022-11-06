using UnityEngine;

namespace Boss
{
    public class EnemyBoss: Enemy
    {
        public float secondsBeforeNextBullet = 0.2f;
        public float bulletsMagazineSize = 10;
        
        private float _timeForNextMagazine;
        private float _timeForNextBullet;
        private bool _isShooting;
        private int _currentBulletNumber = 1;
        
        private void Update()
        {
            if (_isShooting)
            {
                _timeForNextBullet += Time.deltaTime;
                if (!(_timeForNextBullet >= secondsBeforeNextBullet)) return;
                _timeForNextBullet -= secondsBeforeNextBullet;
                ShootNextBullet();
                return;   
            }
            
            _timeForNextMagazine += Time.deltaTime;
            if (!(_timeForNextMagazine >= secondsBeforeNextShooting)) return;
            _timeForNextMagazine -= secondsBeforeNextShooting;
            _isShooting = true;
        }

        private void ShootNextBullet()
        {
            if (_currentBulletNumber > bulletsMagazineSize)
            {
                _currentBulletNumber = 1;
                _isShooting = false;
                return;
            }

            Debug.Log("TA!");
            Shoot();
            _currentBulletNumber++;
        }

        protected override void OnDamage()
        {
            Debug.Log("A!");
        }
    }
}