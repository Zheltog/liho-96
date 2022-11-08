using System.Collections;
using Common;
using UnityEngine;

namespace Boss
{
    public class Courier : MonoBehaviour
    {
        public AudioController player;
        public HealthBar hpBar;
        public GameObject redPanel;
        public GameObject gun;
        public MainController mainController;
        public float damage = 10f;
        public float maxCameraHeight = 5f;
        public float minCameraHeight = 2f;
        public float cameraSpeed = 10f;
        public float hitAnimationSeconds = 0.1f;
        public float shake = 0.2f;
        public float goingDownSpeed = 10f;
        public float secondsBeforeNextShot = 0.5f;

        private Animator _animator;
        private Animator _gunAnimator;
        private Camera _camera;
        private bool _isGoingDown;
        private bool _isRest = true;
        private float _currentTime;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _gunAnimator = gun.GetComponent<Animator>();
            _camera = GetComponentInChildren<Camera>();
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

            _currentTime += Time.deltaTime;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!(_currentTime >= secondsBeforeNextShot)) return;
                _currentTime -= secondsBeforeNextShot;
                Shoot();
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
            Debug.Log("STOP REST");
            _isRest = false;
            gun.SetActive(true);
        }

        public void Rest()
        {
            Debug.Log("REST");
            _isRest = true;
            _isGoingDown = true;
            gun.SetActive(false);
        }
        
        public void Hit(float damageTaken)
        {
            if (!IsActivePhase())
            {
                return;
            }

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

            var newY = transform.position.y + movement.y;

            if (newY > maxCameraHeight)
            {
                newY = maxCameraHeight;
            }

            if (newY < minCameraHeight)
            {
                newY = minCameraHeight;
            }

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void Shoot()
        {
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
        }

        private IEnumerator ShowHitAnimation()
        {
            redPanel.SetActive(true);
            
            var positionBefore = transform.position;
            transform.position = new Vector3(positionBefore.x, positionBefore.y, positionBefore.z - shake);
            yield return new WaitForSeconds(hitAnimationSeconds);
            
            var positionAfter = transform.position;
            transform.position = new Vector3(positionAfter.x, positionAfter.y, positionAfter.z + shake);
            redPanel.SetActive(false);
        }

        private bool IsActivePhase()
        {
            return mainController.CurrentFightState == MainController.FightState.Attack;
        }
    }
}