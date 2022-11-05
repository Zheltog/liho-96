using System.Collections;
using UnityEngine;

namespace Boss
{
    public class Enemy : CharacterBehindBench
    {
        public Vector3 targetPoint = new Vector3(0f, 5f, -10f);
        public float secondsBeforeNextShot = 2f;
        public float maxHidingSeconds = 3f;
        public float minHidingSeconds = 0.5f;
        public float minXPoint = -20f;
        public float maxXPoint = -10f;
        public float minHeight = 2;
        public float maxHeight = 5;
        public float goingVerticalSpeed = 10f;

        private float _currentTime;
        private VerticalMovement _currentMovement = VerticalMovement.None;

        private void Update()
        {
            GoVerticalIfShould();
            
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= secondsBeforeNextShot)) return;
            _currentTime -= secondsBeforeNextShot;
            Shoot();
        }

        private void Shoot()
        {
            var fromPosition = transform.position;
            var direction = targetPoint - fromPosition;
            var ray = new Ray(transform.position, direction);
            Shoot(ray);
        }

        private IEnumerator HideAndMove()
        {
            _currentMovement = VerticalMovement.Down;
            yield return new WaitUntil(() => _currentMovement != VerticalMovement.None);
            yield return new WaitForSeconds(RandomHidingTime());
            var positionWhenHidden = transform.position;
            transform.position = new Vector3(RandomXPosition(), positionWhenHidden.y, positionWhenHidden.z);
            _currentMovement = VerticalMovement.Up;
            yield return new WaitUntil(() => _currentMovement != VerticalMovement.None);
        }

        private float RandomHidingTime()
        {
            return Random.Range(minHidingSeconds, maxHidingSeconds);
        }

        private float RandomXPosition()
        {
            return Random.Range(minXPoint, maxXPoint);
        }

        private void GoVerticalIfShould()
        {
            if (_currentMovement == VerticalMovement.None)
            {
                return;
            }

            var currentPosition = transform.position;
            float newY = currentPosition.y;

            switch (_currentMovement)
            {
                case VerticalMovement.Up:
                    newY += Time.deltaTime * goingVerticalSpeed;
                    break;
                case VerticalMovement.Down:
                    newY -=  Time.deltaTime * goingVerticalSpeed;
                    break;
            }

            if (newY <= minHeight)
            {
                newY = minHeight;
                _currentMovement = VerticalMovement.None;
            }
            else if (newY >= maxHeight)
            {
                newY = maxHeight;
                _currentMovement = VerticalMovement.None;
            }

            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }

        protected override bool ShouldTakeDamage()
        {
            return true;
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }

        protected override void OnDamage()
        {
            StartCoroutine(HideAndMove());
        }

        private enum VerticalMovement
        {
            Up, Down, None
        }
    }
}