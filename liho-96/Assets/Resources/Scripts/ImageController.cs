using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public float darkingSeconds = 1f;
    
    private Image _image;
    private Animator _animator;

    private void Start()
    {
        _image  = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }
    
    public void NewImage(string imageName)
    {
        if (imageName == null) return;
        
        _animator.SetBool("lighting", false);
        _animator.SetBool("darking", true);
        StartCoroutine(WaitAndSetNewImage(imageName));
    }

    private IEnumerator WaitAndSetNewImage(string imageName)
    {
        yield return new WaitForSeconds(darkingSeconds);
        var sprite = Resources.Load<Sprite>("Images/" + imageName);
        _image.sprite = sprite;
        _animator.SetBool("darking", false);
        _animator.SetBool("lighting", true);
    }
}