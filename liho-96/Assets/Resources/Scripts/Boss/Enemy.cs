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

        private float _currentTime;

        private void Update()
        {
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
            var positionBefore = transform.position;
            transform.position = new Vector3(positionBefore.x, minHeight, positionBefore.z);
            yield return new WaitForSeconds(RandomHidingTime());
            var positionAfter = transform.position;
            transform.position = new Vector3(RandomXPosition(), maxHeight, positionAfter.z);
        }

        private float RandomHidingTime()
        {
            return Random.Range(minHidingSeconds, maxHidingSeconds);
        }

        private float RandomXPosition()
        {
            return Random.Range(minXPoint, maxXPoint);
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
    }
}