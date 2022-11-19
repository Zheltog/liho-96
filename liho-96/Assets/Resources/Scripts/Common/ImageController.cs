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
        /// Сразу устанавливает изображение, без анимации затухания.
        /// Нужно для начала игры и переходов между сценами.
        /// </summary>
        public void NewImageImmediate(string imageName)
        {
            if (!_initialized)
            {
                Start();
            }
            
            if (imageName == null || imageName == _lastImageName)
            {
                return;
            }
            
            setImage(imageName);
        }
    
        /// <summary>
        /// Плавно переключает старое изображение на новое.
        /// Если это одно и то же изображение, анимация не проигрывается.
        /// </summary>
        public void NewImage(string imageName)
        {
            if (!_initialized)
            {
                Start();
            }
            
            if (imageName == null || imageName == _lastImageName)
            {
                return;
            }

            _animator.SetBool("lighting", false);
            _animator.SetBool("darking", true);
            StartCoroutine(WaitAndSetNewImage(imageName));
        }

        private IEnumerator WaitAndSetNewImage(string imageName)
        {
            yield return new WaitForSeconds(darkingSeconds);
            
            setImage(imageName);
            
            _animator.SetBool("darking", false);
            _animator.SetBool("lighting", true);
        }

        private void setImage(string imageName)
        {
            var sprite = Resources.Load<Sprite>("Images/" + imageName);
            _image.sprite = sprite;
            _lastImageName = imageName;
        }
    }
}