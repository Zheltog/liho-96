using System.Collections;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Final
{
    public class MainController : MonoBehaviour
    {
        public TMP_InputField loginInput;
        public TMP_InputField passInput;
        public TextMeshProUGUI text;

        public void OnButtonClick()
        {
            var login = loginInput.text;
            var pass = passInput.text;

            var error = ErrorOfInput(login, pass);

            if (error != null)
            {
                text.text = error;
                return;
            }

            text.text = "Отправлен запрос на авторизацию пользователя " + login;
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
            } else {
                var token = uwr.downloadHandler.text;
            }
        }

        [CanBeNull]
        private string ErrorOfInput(string login, string pass)
        {
            if (string.IsNullOrEmpty(login))
            {
                return "Друг, задай логин.";
            }
            
            if (string.IsNullOrEmpty(pass))
            {
                return "Друг, задай пароль.";
            }

            return null;
        }
    }
}