using System;
using JetBrains.Annotations;
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

        public string CheckInfo()
        {
            return InputErrorOf(numberInput.text, monthInput.text, yearInput.text, cvvInput.text);
        }
        
        [CanBeNull]
        private string InputErrorOf(string number, string month, string year, string cvv)
        {
            if (string.IsNullOrEmpty(number) || number.Length != 16)
            {
                return "Номер карты вписывай... <харчок> ...блядина!";
            }
            
            if (string.IsNullOrEmpty(month) ||
                string.IsNullOrEmpty(year) ||
                (month[0] == '0' && month.Length == 1) ||
                (month[0] != '0' && int.Parse(month) > 12) ||
                (year[0] == '0' && year.Length == 1))
            {
                return "Дату нормально введи... <харчок> ...блядина!";
            }
            
            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3)
            {
                return "CVV тоже надо... <харчок> ...блядина!";
            }

            return null;
        }
    }
}