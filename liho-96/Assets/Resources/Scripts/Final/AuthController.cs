using System.Collections;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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
            if (ValidateInput(login, pass))
            {
                StartCoroutine(Login(login, pass));
            }
        }
        
        private IEnumerator Login(string login, string password) {
            var uwr = UnityWebRequest.Post(ApiInfoHolder.QuestDomain + ApiInfoHolder.LoginPath, "");
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new LoginRequest(login, password))
            ));
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.Log(uwr.error);
                _mainController.Error(CommentsHolder.AuthError);
            } else {
                var responseString = uwr.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                StateHolder.Token = response.token;
                Debug.Log("token = " + StateHolder.Token);
                _mainController.Success();
            }
        }
        
        private bool ValidateInput(string login, string pass)
        {
            if (string.IsNullOrEmpty(login))
            {
                _mainController.Error(CommentsHolder.LoginEmpty);
                return false;
            }
            
            if (string.IsNullOrEmpty(pass))
            {
                _mainController.Error(CommentsHolder.PassEmpty);
                return false;
            }

            return true;
        }
    }
}