using System.Collections;
using System.Text;
using JetBrains.Annotations;
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

        [CanBeNull]
        public string TryAuthError()
        {
            var login = loginInput.text;
            var pass = passInput.text;
            var inputError = InputErrorOf(login, pass);
            
            return inputError;
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
        private string InputErrorOf(string login, string pass)
        {
            if (string.IsNullOrEmpty(login))
            {
                return "Почту вписывай... <харчок> ...блядина!";
            }
            
            if (string.IsNullOrEmpty(pass))
            {
                return "Пароль выкладывай... <харчок> ...блядина!";
            }

            return null;
        }
    }
}