using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image  = gameObject.GetComponent<Image>();
    }
    
    public void NewImage()
    {
        var nextImage = GameStateHolder.CurrentFrame.Picture;
        if (nextImage == null)
        {
            return;
        }
        
        var sprite = Resources.Load<Sprite>("Images/" + nextImage);
        _image.sprite = sprite;
    }
}