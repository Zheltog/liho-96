using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float max = 5f;
    private float min = 2f;

    private float speed = 10f;

    private Camera _camera;

    private void Start() {
        _camera = GetComponent<Camera>();
    }
    
    private void Update() {
        var deltaY = Input.GetAxis("Vertical") * speed;
        var movement = transform.TransformDirection(
            Vector3.ClampMagnitude(new Vector3(0f, deltaY, 0f), speed) * Time.deltaTime
        );
        
        var newY = transform.position.y + movement.y;
        
        if (newY > max)
        {
            newY = max;
        }
        
        if (newY < min)
        {
            newY = min;
        }

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        
        ShootDetection();
    }

    private void ShootDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit " + hit.collider.gameObject.name);
            }
        }
    }
}