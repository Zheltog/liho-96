using System.Collections;
using UnityEngine;

namespace Boss
{
    public class EnemyRunning: Enemy
    {
        public RunningDirection startDirection;
        public float minX = -22f;
        public float maxX = 22f;
        public float speed = 3f;
        public float middleX;
        public float middleDelta = 0.1f;
        public float minStopSeconds = 1f;
        public float maxStopSeconds = 2f;

        private bool _ignoreMiddleDelta;
        private float _currentTime;
        private RunningDirection _currentDirection;

        private new void Start()
        {
            base.Start();
            _currentDirection = startDirection;
        }

        private void Update()
        {
            Run();
            
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShooting)) return;
            _currentTime -= secondsBeforeNextShooting;
            Shoot();
        }

        private void Run()
        {
            var currentPosition = transform.position;

            if (currentPosition.x <= middleX + middleDelta
                && currentPosition.x >= middleX - middleDelta
                && !_ignoreMiddleDelta)
            {
                StartCoroutine(RandomStop());
            }
            
            switch (_currentDirection)
            {
                case RunningDirection.Left:
                    if (currentPosition.x <= minX)
                    {
                        _currentDirection = RunningDirection.Right;
                        _ignoreMiddleDelta = false;
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
                        _ignoreMiddleDelta = false;
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

        private IEnumerator RandomStop()
        {
            _currentDirection = RunningDirection.None;
            _ignoreMiddleDelta = true;
            yield return new WaitForSeconds(RandomStopTime());
            SetRandomDirection();
        }
        
        private void SetRandomDirection()
        {
            _currentDirection = Random.Range(0, 2) == 0 ?
                RunningDirection.Left : RunningDirection.Right;
        }
        
        private float RandomStopTime()
        {
            return Random.Range(minStopSeconds, maxStopSeconds);
        }

        protected override void OnReset()
        {
            _ignoreMiddleDelta = false;

            var currentPosition = transform.position;
            float newX = minX;

            switch (startDirection)
            {
                case RunningDirection.Left:
                    newX = maxX;
                    break;
                case RunningDirection.Right:
                    newX = minX;
                    break;
            }

            transform.position = new Vector3(newX, currentPosition.y, currentPosition.z);
            _currentDirection = startDirection;
        }

        protected override void OnDamage()
        {
            // TODO
            Debug.Log("A!");
        }

        public enum RunningDirection
        {
            Left, Right, None
        }
    }
}