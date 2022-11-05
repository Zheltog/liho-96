using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossFightItemsChoicesController : MonoBehaviour
{
    public BossFightController _bossFightController;

    public TextMeshProUGUI textOnButton1;
    public TextMeshProUGUI textOnButton2;
    public TextMeshProUGUI textOnButton3;
    public TextMeshProUGUI textOnButton4;
    public TextMeshProUGUI textOnButton5;
    public TextMeshProUGUI textOnButton6;
    
    private List<Item> _currentItems;

    public bool ItemsChoiceAvailable()
    {
        return _currentItems.Count != 0;
    }
    
    private void Start()
    {
        InitHolder();
        _currentItems = BossFightStateHolder.AvailableItems;
    }

    public void NewChoices()
    {
        if (!ItemsChoiceAvailable())
        {
            return;
        }

        textOnButton1.text = FormattedText(_currentItems[0].Name);
        textOnButton1.transform.parent.gameObject.SetActive(true);
        
        if (_currentItems.Count > 1)
        {
            textOnButton2.text = FormattedText(_currentItems[1].Name);
            textOnButton2.transform.parent.gameObject.SetActive(true);
        }
        if (_currentItems.Count > 2)
        {
            textOnButton3.text = FormattedText(_currentItems[2].Name);
            textOnButton3.transform.parent.gameObject.SetActive(true);
        }
        if (_currentItems.Count > 3)
        {
            textOnButton4.text = FormattedText(_currentItems[3].Name);
            textOnButton4.transform.parent.gameObject.SetActive(true);
        }
        if (_currentItems.Count > 4)
        {
            textOnButton5.text = FormattedText(_currentItems[4].Name);
            textOnButton5.transform.parent.gameObject.SetActive(true);
        }
        if (_currentItems.Count > 5)
        {
            textOnButton6.text = FormattedText(_currentItems[5].Name);
            textOnButton6.transform.parent.gameObject.SetActive(true);
        }
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

    private void ChooseItem(int index)
    {
        var chosenItem = _currentItems[index];
        _bossFightController.ChooseItem(chosenItem);
        _currentItems.Remove(chosenItem);
    }
    
    // TODO: удалить, нужен для тестирования
    private void InitHolder()
    {
        var items = new List<Item>();
        items.Add(new Item("Вейп", "<Курьер оформляет плотнейшего пыха и восстанавливает 10 очков здоровья>", "флаг", new Effect(EffectType.Heal, 10f)));
        items.Add(new Item("Журнал \"Playboy\"", "<Курьер пристально всматривается в обложку журнала. Вспомнив, за что должно сражать настоящему мужчине, он издаёт свирепый рык, нанося противникам 10 очков урона>", "флаг", new Effect(EffectType.Damage, 10f)));
        BossFightStateHolder.Init(items, null);
    }
    
    private static string FormattedText(string original)
    {
        return "> " + original;
    }
}