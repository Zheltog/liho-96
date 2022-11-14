using System;
using System.Collections;
using System.Linq;
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
            if (string.IsNullOrEmpty(number) || number.Length != 16 || 
                !long.TryParse(number, out var cardResult) || cardResult < 1 ||
                !ChecksumLuhnAlgorithm(number))
            {
                _mainController.Error(CommentsHolder.InvalidCard);
                return false;
            }
            
            // TODO некрасиво и трудно расширять
            if (string.IsNullOrEmpty(month) ||
                string.IsNullOrEmpty(year) ||
                int.Parse(month) > 12 || int.Parse(month) < 1 ||
                int.Parse(year) < 22)
            {
                _mainController.Error(CommentsHolder.InvalidDate);
                return false;
            }

            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3 || !int.TryParse(cvv, out var cvvResult) || cvvResult < 0)
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

        private bool ChecksumLuhnAlgorithm(string cardNumber)
        {
            var charToDigit = new Func<char, int>(c => c - 48);

            return cardNumber.All(char.IsDigit) &&
                   cardNumber.Reverse()
                       .Select(charToDigit)
                       .Select((thisNum, index) => index % 2 == 0
                           ? thisNum
                           : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum))
                       .Sum() % 10 == 0;
        }
        
        private IEnumerator CheckAnswer() {
            var uwr = UnityWebRequest.Post(ApiInfoHolder.QuestDomain + ApiInfoHolder.CheckTaskPath, "");
            var responseJson = JsonUtility.ToJson(new CheckTaskRequest(ApiInfoHolder.TaskId, ApiInfoHolder.Answer));
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(responseJson));
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", "Bearer " + StateHolder.Token);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.Log(uwr.error);
                _mainController.Error(CommentsHolder.CheckTaskError);
            } else {
                var responseString = uwr.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<CheckTaskResponse>(responseString);
                if (response.points != null)
                {
                    Debug.Log(response.points);
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