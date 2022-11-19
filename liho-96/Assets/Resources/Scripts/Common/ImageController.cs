using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Frames
{
    public class ImageController : MonoBehaviour
    {
        public float darkingSeconds = 0.5f;
    
        private Image _image;
        private Animator _animator;
        private string _lastImageName;
        private bool _initialized;

        private void Start()
        {
            _image  = GetComponent<Image>();
            _animator = GetComponent<Animator>();
            _initialized = true;
        }

        /// <summary>
        /// Плавно переключает старое изображение на новое.
        /// Если это одно и то же изображение, анимация не проигрывается.
        /// </summary>
        public void NewImage(string imageName, NewImageLoadType loadType)
        {
            if (!_initialized)
            {
                Start();
            }
            
            if (imageName == null || imageName == _lastImageName)
            {
                return;
            }

            switch (loadType)
            {
                case NewImageLoadType.DarkerThenLighter:
                    DimImage();
                    StartCoroutine(WaitAndSetNewImage(imageName));
                    break;
                case NewImageLoadType.JustLighter:
                    SetNewImage(imageName);
                    break;
            }
        }

        public void DimImage()
        {
            _animator.SetBool("lighting", false);
            _animator.SetBool("darking", true);
        }

        private IEnumerator WaitAndSetNewImage(string imageName)
        {
            yield return new WaitForSeconds(darkingSeconds);
            
            _animator.SetBool("darking", false);
            SetNewImage(imageName);
        }

        private void SetNewImage(string imageName)
        {
            SetImage(imageName);
            _animator.SetBool("lighting", true);
        }

        private void SetImage(string imageName)
        {
            var sprite = Resources.Load<Sprite>("Images/" + imageName);
            _image.sprite = sprite;
            _lastImageName = imageName;
        }

        public enum NewImageLoadType
        {
            DarkerThenLighter, JustLighter
        }
    }
}