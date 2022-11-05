using UnityEngine;

public class Courier : MonoBehaviour
{

    public float damage = 5;

    private float maxCameraHeight = 5f;
    private float minCameraHeight = 2f;
    private float cameraSpeed = 10f;

    private Camera _camera;

    private void Start() {
        _camera = GetComponent<Camera>();
    }
    
    private void Update() {
        UpdateCamera();
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
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
}