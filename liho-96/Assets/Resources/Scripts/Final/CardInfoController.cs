using TMPro;
using UnityEngine;

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
            ValidateCardInfo(numberInput.text, monthInput.text, yearInput.text, cvvInput.text);
            // TODO: запрос
        }
        
        private void ValidateCardInfo(string number, string month, string year, string cvv)
        {
            if (string.IsNullOrEmpty(number) || number.Length != 16)
            {
                _mainController.Error(CommentsHolder.InvalidCard);
                return;
            }
            
            if (string.IsNullOrEmpty(month) ||
                string.IsNullOrEmpty(year) ||
                (month[0] == '0' && month.Length == 1) ||
                (month[0] != '0' && int.Parse(month) > 12) ||
                (year[0] == '0' && year.Length == 1))
            {
                _mainController.Error(CommentsHolder.InvalidDate);
                return;
            }
            
            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3)
            {
                _mainController.Error(CommentsHolder.InvalidCvv);
                return;
            }
            
            _mainController.Success();  // TODO: убрать
        }
    }
}