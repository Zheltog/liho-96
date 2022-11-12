using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;

namespace Boss
{
    public class ItemsChoicesController : MonoBehaviour
    {
        public MainController mainController;
        public GameObject backButton;
        public GameObject forwardButton;
        // TODO: сгенерировать кнопки по-человечески?
        public TextMeshProUGUI textOnButton1;
        public TextMeshProUGUI textOnButton2;
        public TextMeshProUGUI textOnButton3;
        public TextMeshProUGUI textOnButton4;
        public TextMeshProUGUI textOnButton5;
        public TextMeshProUGUI textOnButton6;

        private List<Item> _currentItems;
        private int pageIndex;

        private void Start()
        {
            _currentItems = StateHolder.AvailableItems;
        }

        public void Back()
        {
            if (pageIndex == 0) return;
            DisableButtons();
            pageIndex--;
            NewChoices();
        }

        public void Forward()
        {
            if (pageIndex + 1 == PagesAvailable()) return;
            DisableButtons();
            pageIndex++;
            NewChoices();
        }

        public void NewChoices()
        {
            if (!ItemsChoiceAvailable())
            {
                return;
            }

            if (pageIndex > 0) backButton.SetActive(true);
            if (_currentItems.Count > pageIndex * 6 + 6) forwardButton.SetActive(true);

            textOnButton1.text = Utils.FormatButtonText(_currentItems[pageIndex * 6].Name);
            textOnButton1.transform.parent.gameObject.SetActive(true);

            if (_currentItems.Count > pageIndex * 6 + 1)
            {
                textOnButton2.text = Utils.FormatButtonText(_currentItems[pageIndex * 6 + 1].Name);
                textOnButton2.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > pageIndex * 6 + 2)
            {
                textOnButton3.text = Utils.FormatButtonText(_currentItems[pageIndex * 6 + 2].Name);
                textOnButton3.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > pageIndex * 6 + 3)
            {
                textOnButton4.text = Utils.FormatButtonText(_currentItems[pageIndex * 6 + 3].Name);
                textOnButton4.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count > pageIndex * 6 + 4)
            {
                textOnButton5.text = Utils.FormatButtonText(_currentItems[pageIndex * 6 + 4].Name);
                textOnButton5.transform.parent.gameObject.SetActive(true);
            }

            if (_currentItems.Count <= pageIndex * 6 + 5) return;
            
            textOnButton6.text = Utils.FormatButtonText(_currentItems[pageIndex * 6 + 5].Name);
            textOnButton6.transform.parent.gameObject.SetActive(true);
        }

        public void DisableButtons()
        {
            backButton.SetActive(false);
            forwardButton.SetActive(false);
            textOnButton1.transform.parent.gameObject.SetActive(false);
            textOnButton2.transform.parent.gameObject.SetActive(false);
            textOnButton3.transform.parent.gameObject.SetActive(false);
            textOnButton4.transform.parent.gameObject.SetActive(false);
            textOnButton5.transform.parent.gameObject.SetActive(false);
            textOnButton6.transform.parent.gameObject.SetActive(false);
        }

        public void ChooseFirst()
        {
            ChooseItem(pageIndex * 6 + 0);
        }

        public void ChooseSecond()
        {
            ChooseItem(pageIndex * 6 + 1);
        }

        public void ChooseThird()
        {
            ChooseItem(pageIndex * 6 + 2);
        }
        
        public void ChooseFourth()
        {
            ChooseItem(pageIndex * 6 + 3);
        }
        
        public void ChooseFifth()
        {
            ChooseItem(pageIndex * 6 + 4);
        }
        
        public void ChooseSixth()
        {
            ChooseItem(pageIndex * 6 + 5);
        }
        
        public bool ItemsChoiceAvailable()
        {
            return _currentItems.Count != 0;
        }

        private void ChooseItem(int index)
        {
            var chosenItem = _currentItems[index];
            mainController.ChooseItem(chosenItem);
            _currentItems.Remove(chosenItem);
            pageIndex = 0;
        }

        private int PagesAvailable()
        {
            var pages = _currentItems.Count / 6;
            if (_currentItems.Count % 6 != 0)
            {
                pages++;
            }

            return pages;
        }
    }
}