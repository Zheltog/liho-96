using System.Collections;
using UnityEngine;

namespace Boss
{
    public class Courier : MonoBehaviour
    {
        public HealthBar hpBar;
        public GameObject redPanel;
        public MainController mainController;
        public float damage = 10f;
        public float maxCameraHeight = 5f;
        public float minCameraHeight = 2f;
        public float cameraSpeed = 10f;
        public float hitAnimationSeconds = 0.1f;
        public float shake = 0.2f;
        public float goingDownSpeed = 10f;

        private Animator _animator;
        private Camera _camera;
        private bool _isGoingDown;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _camera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (!IsActivePhase())
            {
                if (_isGoingDown)
                {
                    GoDown();
                }
                
                return;
            }

            UpdateCamera();

            if (Input.GetMouseButtonDown(0))
            {
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

        public void Rest()
        {
            _isGoingDown = true;
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
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            var enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
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