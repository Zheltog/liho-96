using System;
using UnityEngine;

namespace Boss
{
    public class EnemyBoss: Enemy
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