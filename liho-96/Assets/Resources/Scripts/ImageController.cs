using UnityEngine;
using UnityEngine.UIElements;

public class ImageController : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        SetImage("car_driving_on_a_rainy_road");
    }
    
    public void SetImage(string imageName)
    {
        var sprite = Resources.Load<Texture2D>("Images/" + imageName);
        _image.image = sprite;
    }
}