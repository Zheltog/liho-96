using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class EnemiesController : MonoBehaviour
    {
        public Enemy enemyBenchLeft;
        public Enemy enemyBenchRight;
        public Enemy enemyRunningLeft;
        public Enemy enemyRunningRight;
        public Enemy vlada;
        
        public void DisableAllEnemies()
        {
            enemyBenchLeft.gameObject.SetActive(false);
            enemyBenchRight.gameObject.SetActive(false);
            enemyRunningLeft.gameObject.SetActive(false);
            enemyRunningRight.gameObject.SetActive(false);
            vlada.gameObject.SetActive(false);
        }
        
        public void SetEnemies(List<EnemyType> enemies)
        {
            if (enemies.Contains(EnemyType.BenchLeft))
            {
                enemyBenchLeft.gameObject.SetActive(true);
                enemyBenchLeft.Reset();
            }
            if (enemies.Contains(EnemyType.BenchRight))
            {
                enemyBenchRight.gameObject.SetActive(true);
                enemyBenchRight.Reset();
            }
            if (enemies.Contains(EnemyType.RunningLeft))
            {
                enemyRunningLeft.gameObject.SetActive(true);
                enemyRunningLeft.Reset();
            }
            if (enemies.Contains(EnemyType.RunningRight))
            {
                enemyRunningRight.gameObject.SetActive(true);
                enemyRunningRight.Reset();
            }
            if (enemies.Contains(EnemyType.Vlada))
            {
                vlada.gameObject.SetActive(true);
                vlada.Reset();
            }
        }
    }
}