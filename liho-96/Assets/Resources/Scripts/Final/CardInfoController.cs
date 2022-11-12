using System.Collections;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Final
{
    public class CardInfoController : MonoBehaviour
    {
        public TMP_InputField numberInput;
        public TMP_InputField monthInput;
        public TMP_InputField yearInput;
        public TMP_InputField cvvInput;

        private MainController _mainController;

        private void Start()
        {
            _mainController = GetComponent<MainController>();
        }

        public void CheckInfo()
        {
            if (ValidateCardInfo(numberInput.text, monthInput.text, yearInput.text, cvvInput.text))
            {
                StartCoroutine(CheckAnswer());
            }
        }
        
        private bool ValidateCardInfo(string number, string month, string year, string cvv)
        {
            if (string.IsNullOrEmpty(number) || number.Length != 16)
            {
                _mainController.Error(CommentsHolder.InvalidCard);
                return false;
            }
            
            if (string.IsNullOrEmpty(month) ||
                string.IsNullOrEmpty(year) ||
                (month[0] == '0' && month.Length == 1) ||
                (month[0] != '0' && int.Parse(month) > 12) ||
                (year[0] == '0' && year.Length == 1))
            {
                _mainController.Error(CommentsHolder.InvalidDate);
                return false;
            }
            
            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3)
            {
                _mainController.Error(CommentsHolder.InvalidCvv);
                return false;
            }
            
            if (StateHolder.Token == null)
            {
                _mainController.Success();
                return false;
            }
            
            return true;
        }
        
        private IEnumerator CheckAnswer() {
            var uwr = UnityWebRequest.Post(ApiInfoHolder.QuestDomain + ApiInfoHolder.CheckTaskPath, "");
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new CheckTaskRequest(ApiInfoHolder.TaskId, StateHolder.Token))
            ));
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.Log(uwr.error);
                _mainController.Error(CommentsHolder.CheckTaskError);
            } else {
                var responseString = uwr.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<CheckTaskResponse>(responseString);
                if (response.points != null)
                {
                    _mainController.Success();
                }
                else
                {
                    _mainController.Error(CommentsHolder.CheckTaskError);
                }
            }
        }
    }
}