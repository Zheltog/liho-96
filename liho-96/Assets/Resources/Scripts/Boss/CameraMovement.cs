using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float max = 3f;
    private float min = 1;

    private float speed = 10f;

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
    }
}