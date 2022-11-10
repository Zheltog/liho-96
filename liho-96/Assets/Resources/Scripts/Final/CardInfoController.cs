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

        public bool CheckInfo()
        {
            return false;
        }
    }
}