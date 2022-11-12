using System.Collections;
using Common;
using UnityEngine;

namespace Boss
{
    public class Courier : MonoBehaviour
    {
        public CursorController cursorController;
        public AudioController player;
        public HealthBar hpBar;
        public GameObject redPanel;
        public GameObject gun;
        public float damage = 10f;
        public float coolPistolBonus = 0.5f;
        public float maxCameraHeight = 5f;
        public float minCameraHeight = 2f;
        public float cameraSpeed = 10f;
        public float hitAnimationSeconds = 0.1f;
        public float shakeOnHitDistance = 0.2f;
        public float goingDownSpeed = 10f;
        public float secondsBeforeNextShot = 0.5f;
        public float minShootingHeight = 3f;

        private Animator _animator;
        private Animator _gunAnimator;
        private Camera _camera;
        private bool _isGoingDown;
        private bool _isRest = true;
        private bool _fireAllowed = true;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _gunAnimator = gun.GetComponent<Animator>();
            _camera = GetComponentInChildren<Camera>();
            StartCoroutine(CheckCoolPistol());
        }

        private void Update()
        {
            if (_isRest)
            {
                if (_isGoingDown)
                {
                    GoDown();
                }
                
                return;
            }

            UpdateCamera();

            if (Input.GetMouseButtonDown(0) && ShootingAvailable())
            {
                StartCoroutine(Shoot());
            }
        }

        public void CameraShake(bool isShaking)
        {
            if (isShaking)
            {
                _animator.Play("CameraShake");
            }
            else
            {
                _animator.Rebind();
                _animator.Update(0f);
            }
        }

        public void StopRest()
        {
            _isRest = false;
            gun.SetActive(true);
        }

        public void Rest()
        {
            _isRest = true;
            _isGoingDown = true;
            gun.SetActive(false);
        }
        
        public void Hit(float damageTaken)
        {
            hpBar.AddHp(-1 * damageTaken, false);
            StartCoroutine(ShowHitAnimation());
        }

        private void GoDown()
        {
            var currentPosition = transform.position;
            var newY = currentPosition.y - goingDownSpeed * Time.deltaTime;

            if (newY <= minCameraHeight)
            {
                newY = minCameraHeight;
                _isGoingDown = false;
            }
            
            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }

        private void UpdateCamera()
        {
            var deltaY = Input.GetAxis("Vertical") * cameraSpeed;
            var movement = transform.TransformDirection(
                Vector3.ClampMagnitude(new Vector3(0f, deltaY, 0f), cameraSpeed) * Time.deltaTime
            );

            var currentPosition = transform.position;
            var newY = currentPosition.y + movement.y;

            if (newY > maxCameraHeight)
            {
                newY = maxCameraHeight;
            }

            if (newY < minCameraHeight)
            {
                newY = minCameraHeight;
            }

            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }

        private IEnumerator Shoot()
        {
            _fireAllowed = false;
            
            _gunAnimator.Rebind();
            _gunAnimator.Update(0f);
            
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay (mousePosition);
            var hit = Physics2D.GetRayIntersection(ray);
            var enemy = hit.collider?.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
            
            _gunAnimator.Play("GunShake");
            player.NewSound("shot");

            yield return new WaitForSeconds(secondsBeforeNextShot);
            _fireAllowed = true;
        }

        // TODO: нормальная анимация?
        private IEnumerator ShowHitAnimation()
        {
            redPanel.SetActive(true);
            
            var positionBefore = transform.position;
            transform.position = new Vector3(
                positionBefore.x, positionBefore.y, positionBefore.z - shakeOnHitDistance
            );
            
            yield return new WaitForSeconds(hitAnimationSeconds);
            
            var positionAfter = transform.position;
            transform.position = new Vector3(
                positionAfter.x, positionAfter.y, positionAfter.z + shakeOnHitDistance
            );
            
            redPanel.SetActive(false);
        }

        private bool ShootingAvailable()
        {
            var mousePosition = Input.mousePosition;
            var mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
            return transform.position.y >= minShootingHeight &&
                   _fireAllowed &&
                   cursorController.IsCursorInAimRect(mousePosition2D);
        }

        private IEnumerator CheckCoolPistol()
        {
            yield return new WaitForSeconds(0.5f);
            if (StateHolder.CourierCoolPistol)
            {
                damage += coolPistolBonus;
            }
        }
    }
}