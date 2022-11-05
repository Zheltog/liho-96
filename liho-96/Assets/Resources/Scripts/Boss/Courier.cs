using System.Collections;
using UnityEngine;

namespace Boss
{
    public class Courier : MonoBehaviour
    {
        public HealthBar hpBar;
        public GameObject redPanel;
        public MainController mainController;
        public float hp = 100;
        public float damage = 5;
        public float maxCameraHeight = 5f;
        public float minCameraHeight = 2f;
        public float cameraSpeed = 10f;
        public float hitAnimationSeconds = 0.1f;
        public float shake = 0.2f;
        
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (!IsActivePhase())
            {
                return;
            }

            UpdateCamera();

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        public void Rest()
        {
            transform.position = new Vector3(transform.position.x, minCameraHeight, transform.position.z);
        }

        public void Hit(float damageTaken)
        {
            if (!IsActivePhase())
            {
                return;
            }
            
            var hpDamage = damageTaken;

            hp -= damageTaken;

            if (hp <= 0)
            {
                hpDamage = damageTaken + hp;
                mainController.GameOver("Райан Гослинг умер...");
            }

            hpBar.Damage(hpDamage);
            StartCoroutine(ShowHitAnimation());
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
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Hit(damage);
                }
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
            return mainController.CurrentRoundState == MainController.RoundState.Attack;
        }
    }
}