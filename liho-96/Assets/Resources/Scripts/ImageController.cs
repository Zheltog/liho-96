using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image  = gameObject.GetComponent<Image>();
    }
    
    public void NewImage(string imageName)
    {
        if (imageName == null) return;
        var sprite = Resources.Load<Sprite>("Images/" + imageName);
        _image.sprite = sprite;
    }
}