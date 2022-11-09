using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class EnemiesController : MonoBehaviour
    {
        public GameObject enemyBenchLeft;
        public GameObject enemyBenchRight;
        public GameObject enemyRunningLeft;
        public GameObject enemyRunningRight;
        public GameObject vlada;
        
        public void DisableAllEnemies()
        {
            enemyBenchLeft.SetActive(false);
            enemyBenchRight.SetActive(false);
            enemyRunningLeft.SetActive(false);
            enemyRunningRight.SetActive(false);
            vlada.SetActive(false);
        }
        
        public void SetEnemies(List<EnemyType> enemies)
        {
            if (enemies.Contains(EnemyType.BenchLeft))
            {
                enemyBenchLeft.SetActive(true);
            }
            if (enemies.Contains(EnemyType.BenchRight))
            {
                enemyBenchRight.SetActive(true);
            }
            if (enemies.Contains(EnemyType.RunningLeft))
            {
                enemyRunningLeft.SetActive(true);
            }
            if (enemies.Contains(EnemyType.RunningRight))
            {
                enemyRunningRight.SetActive(true);
            }
            if (enemies.Contains(EnemyType.Vlada))
            {
                vlada.SetActive(true);
            }
        }
    }
}