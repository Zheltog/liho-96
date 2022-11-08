using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;

namespace Boss
{
    public class ItemsChoicesController : MonoBehaviour
    {
        public MainController mainController;
        // TODO: сгенерировать кнопки по-человечески?
        public TextMeshProUGUI textOnButton1;
        public TextMeshProUGUI textOnButton2;
        public TextMeshProUGUI textOnButton3;
        public TextMeshProUGUI textOnButton4;
        public TextMeshProUGUI textOnButton5;
        public TextMeshProUGUI textOnButton6;

        private List<Item> _currentItems;

        private void Start()
        {
            _currentItems = StateHolder.AvailableItems;
        }
        
        public bool ItemsChoiceAvailable()
        {
            return _currentItems.Count != 0;
        }

        public void NewChoices()
        {
            if (!ItemsChoiceAvailable())
            {
                return;
            }

            textOnButton1.text = Utils.FormatButtonText(_currentItems[0].Name);
            textOnButton1.transform.parent.gameObject.SetActive(true);

            if (_currentItems.Count > 1)
            {
                textOnButton2.text = Utils.FormatButtonText(_currentItems[1].Name);
                textOnButton2.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > 2)
            {
                textOnButton3.text = Utils.FormatButtonText(_currentItems[2].Name);
                textOnButton3.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > 3)
            {
                textOnButton4.text = Utils.FormatButtonText(_currentItems[3].Name);
                textOnButton4.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > 4)
            {
                textOnButton5.text = Utils.FormatButtonText(_currentItems[4].Name);
                textOnButton5.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count <= 5) return;
            
            textOnButton6.text = Utils.FormatButtonText(_currentItems[5].Name);
            textOnButton6.transform.parent.gameObject.SetActive(true);
        }

        public void DisableButtons()
        {
            textOnButton1.transform.parent.gameObject.SetActive(false);
            textOnButton2.transform.parent.gameObject.SetActive(false);
            textOnButton3.transform.parent.gameObject.SetActive(false);
            textOnButton4.transform.parent.gameObject.SetActive(false);
            textOnButton5.transform.parent.gameObject.SetActive(false);
            textOnButton6.transform.parent.gameObject.SetActive(false);
        }

        public void ChooseFirst()
        {
            ChooseItem(0);
        }

        public void ChooseSecond()
        {
            ChooseItem(1);
        }

        public void ChooseThird()
        {
            ChooseItem(2);
        }
        
        public void ChooseFourth()
        {
            ChooseItem(3);
        }
        
        public void ChooseFifth()
        {
            ChooseItem(4);
        }
        
        public void ChooseSixth()
        {
            ChooseItem(5);
        }

        private void ChooseItem(int index)
        {
            var chosenItem = _currentItems[index];
            mainController.ChooseItem(chosenItem);
            _currentItems.Remove(chosenItem);
        }
    }
}