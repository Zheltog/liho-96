using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Final
{
    public class AuthController : MonoBehaviour
    {
        public TMP_InputField loginInput;
        public TMP_InputField passInput;
        public TextMeshProUGUI text;

        private MainController _mainController;

        private void Start()
        {
            _mainController = GetComponent<MainController>();
        }
        
        public void TryAuth()
        {
            var login = loginInput.text;
            var pass = passInput.text;
            ValidateInput(login, pass);
            // TODO: login
        }
        
        private IEnumerator Login(string login, string password) {
            var uwr = UnityWebRequest.Post("path...", "");
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(
                    JsonUtility.ToJson(new Object()))   // dto
            );
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.Log(uwr.error);
                _mainController.Error(CommentsHolder.AuthError);
            } else {
                var token = uwr.downloadHandler.text;
                // TODO сохранить в холдер
            }
        }
        
        private void ValidateInput(string login, string pass)
        {
            if (string.IsNullOrEmpty(login))
            {
                _mainController.Error(CommentsHolder.LoginEmpty);
                return;
            }
            
            if (string.IsNullOrEmpty(pass))
            {
                _mainController.Error(CommentsHolder.PassEmpty);
                return;
            }
            
            _mainController.Success();  // TODO: убрать
        }
    }
}