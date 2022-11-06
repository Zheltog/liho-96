using UnityEngine;

namespace Boss
{
    public class EnemyRunning: Enemy
    {
        private float _currentTime;
        
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShooting)) return;
            _currentTime -= secondsBeforeNextShooting;
            Shoot();
        }

        protected override void OnDamage()
        {
            Debug.Log("A!");
        }
    }
}