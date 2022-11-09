using UnityEngine;

namespace Boss
{
    public class EnemyRunning: Enemy
    {
        public RunningDirection startDirection;
        public float minX = -22f;
        public float maxX = 22f;
        public float speed = 3f;
        public float middleDelta = 0.3f;

        private float _currentTime;
        private float _middleX;
        private RunningDirection _currentDirection;

        private new void Start()
        {
            base.Start();
            _currentDirection = startDirection;
            _middleX = (minX + maxX) / 2;
        }

        private void Update()
        {
            Run();
            
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShooting)) return;
            _currentTime -= secondsBeforeNextShooting;
            Shoot();
            StartCoroutine(ShowShotBang());
        }
        public override void Reset() {}

        private void Run()
        {
            var currentPosition = transform.position;

            RandomKeepDirection(currentPosition);
            
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

        private void RandomKeepDirection(Vector3 currentPosition)
        {
            if (currentPosition.x <= _middleX + middleDelta
                && currentPosition.x >= _middleX - middleDelta)
            {
                if (Random.Range(0, 2) == 0)
                {
                    InvertDirection();
                }
            }
        }

        private void InvertDirection()
        {
            _currentDirection = _currentDirection == RunningDirection.Left ?
                RunningDirection.Right : RunningDirection.Left;
        }

        protected override void OnDamage()
        {
            // TODO
            Debug.Log("A!");
        }

        public enum RunningDirection
        {
            Left, Right
        }
    }
}