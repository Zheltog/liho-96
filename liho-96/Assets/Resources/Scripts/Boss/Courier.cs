using System.Collections;
using UnityEngine;

public class Courier : MonoBehaviour
{
    public HealthBar hpBar;
    public GameObject redPanel;
    public GameObject grayPanel;

    public float hp = 100;
    public float damage = 5;

    private float _maxCameraHeight = 5f;
    private float _minCameraHeight = 2f;
    private float _cameraSpeed = 10f;
    private float _hitAnimationSeconds = 0.1f;
    private float _shake = 0.2f;
    private bool isRest;

    private Camera _camera;

    private void Start() {
        _camera = GetComponentInChildren<Camera>();
    }
    
    private void Update() {
        if (isRest)
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
        transform.position = new Vector3(transform.position.x, _minCameraHeight, transform.position.z);
        grayPanel.SetActive(true);
        isRest = true;
    }

    public void ReturnToActivePhase()
    {
        grayPanel.SetActive(false);
        isRest = false;
    }
    
    public void Hit(float damage)
    {
        var hpDamage = damage;
        
        hp -= damage;

        if (hp <= 0)
        {
            hpDamage = damage + hp;
            Debug.Log("GAME OVER!!!");
        }
        
        hpBar.Damage(hpDamage);
        StartCoroutine(ShowHitAnimation());
    }

    private void UpdateCamera()
    {
        var deltaY = Input.GetAxis("Vertical") * _cameraSpeed;
        var movement = transform.TransformDirection(
            Vector3.ClampMagnitude(new Vector3(0f, deltaY, 0f), _cameraSpeed) * Time.deltaTime
        );
        
        var newY = transform.position.y + movement.y;
        
        if (newY > _maxCameraHeight)
        {
            newY = _maxCameraHeight;
        }
        
        if (newY < _minCameraHeight)
        {
            newY = _minCameraHeight;
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
        transform.position = new Vector3(positionBefore.x, positionBefore.y, positionBefore.z - _shake);
        yield return new WaitForSeconds(_hitAnimationSeconds);
        var positionAfter = transform.position;
        transform.position = new Vector3(positionAfter.x, positionAfter.y, positionAfter.z + _shake);
        redPanel.SetActive(false);
    }
}