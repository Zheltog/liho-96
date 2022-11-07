using System;
using UnityEngine;

namespace Boss
{
    public class EnemyRunning: Enemy
    {
        public RunningDirection startDirection;
        public float minX = -22f;
        public float maxX = 22f;
        public float speed = 3f;

        private float _currentTime;
        private RunningDirection _currentDirection;

        private void Start()
        {
            _currentDirection = startDirection;
        }

        private void Update()
        {
            Run();
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShooting)) return;
            _currentTime -= secondsBeforeNextShooting;
            Shoot();
            StartCoroutine(ShowShotLight());
        }

        private void Run()
        {
            var currentPosition = transform.position;
            switch (_currentDirection)
            {
                case RunningDirection.Left:
                    if (currentPosition.x <= minX)
                    {
                        _currentDirection = RunningDirection.Right;
                        break;
                    }

                    transform.position = new Vector3(
                        currentPosition.x - speed * Time.deltaTime,
                        currentPosition.y,
                        currentPosition.z
                    );
                    break;
                case RunningDirection.Right:
                    if (currentPosition.x >= maxX)
                    {
                        _currentDirection = RunningDirection.Left;
                        break;
                    }

                    transform.position = new Vector3(
                        currentPosition.x + speed * Time.deltaTime,
                        currentPosition.y,
                        currentPosition.z
                    );
                    break;
            }
        }

        protected override void OnDamage()
        {
            Debug.Log("A!");
        }

        public enum RunningDirection
        {
            Left, Right
        }
    }
}