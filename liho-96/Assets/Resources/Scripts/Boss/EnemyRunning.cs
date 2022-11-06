using UnityEngine;

namespace Boss
{
    public class EnemyRunning: Enemy
    {
        private void Update()
        {
            Shoot();
        }

        protected override void OnDamage()
        {
            Debug.Log("A!");
        }
    }
}