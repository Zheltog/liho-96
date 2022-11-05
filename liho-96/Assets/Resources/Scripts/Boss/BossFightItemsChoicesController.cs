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
    
    // TODO: удалить, нужен для тестирования
    private void InitHolder()
    {
        var items = new List<Item>();
        items.Add(new Item("жопа", "кияяя", "флаг", new Effect(EffectType.Heal, 10f)));
        BossFightStateHolder.Init(items, null);
    }
    
    private static string FormattedText(string original)
    {
        return "> " + original;
    }
}