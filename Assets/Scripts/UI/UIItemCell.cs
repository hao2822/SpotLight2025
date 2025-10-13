using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCell : SerializedMonoBehaviour
{
    public Text ItemNameText;
    public Text KeysText;
    public ItemBase Item;

    public Image Icon;
    public Image FiledImage;
    Tweener  tweener;
    private void OnEnable()
    {
        
    }

    public void AddEvent()
    {
        OnMyItemChanged();
        
        GameManager.OnMyItemChanged -= OnMyItemChanged;
        
        GameManager.OnMyItemChanged += OnMyItemChanged;
    }
    public void SetItem(ItemBase item)
    {
        this.Item = item;
        ItemNameText.text = item.Name;
        KeysText.text = string.Join(",", item.KeyCodes).Replace("Alpha", "");
        Icon.sprite = item.Icon;
        OnMyItemChanged();
        Item.OnStartRemove += OnStartRemove;
        Item.OnCancelRemove += OnCancelRemove;
    }

    private void OnCancelRemove()
    {
        tweener?.Kill();
        tweener = FiledImage.DOFillAmount(1, 0f).From(1).SetEase(Ease.Linear);
        
    }

    private void OnStartRemove()
    {
        tweener?.Kill();
        tweener = FiledImage.DOFillAmount(0, 5f).From(1).SetEase(Ease.Linear);
    }
    uint bonus = 0;
    uint allBonus = 0;
    public Toggle SetToggle(ItemBase item)
    {
        this.Item = item;
        ItemNameText.text = item.Name;
        KeysText.text = $"${item.Price}";
        Icon.sprite = item.Icon;
        var toggle = this.GetComponent<Toggle>();
        toggle.interactable = true;
        GameManager.OnMyItemChanged -= OnMyItemChanged;
        FiledImage.transform.parent.gameObject.SetActive(false);
        var c = GameManager.Instance.Consignors.FirstOrDefault();
        int i = 0;
        if (c != null)
        {
            if (c.Aging == item.Aging)
            {
                i++;
            }

            // if (c.Origin == item.Origin)
            // {
            //     i++;
            // }

            if (c.Material == item.Material)
            {
                i++;
            }

            if (c.Culture == item.Culture)
            {
                i++;
            }

            for (int j = 0; j < i; j++)
            {
                bonus += c.EachTagComparedBonus;
            }

            if (i == 3)
            {
                allBonus = c.AllComparedBonus;
            }
        }
        KeysText.text = $"+${bonus+allBonus}";
        return toggle;
    }

    private void OnMyItemChanged()
    {
        var image = GetComponent<Image>();
        if (GameManager.Instance.MyItems.Contains(Item))
        {
            // image.color = Color.green;
        }
        else
        {
            // image.color = Color.white;
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        GameManager.OnMyItemChanged -= OnMyItemChanged;
        if (Item != null)
        {
            Item.OnStartRemove -= OnStartRemove;
            Item.OnCancelRemove -= OnCancelRemove;
        }
        
    }
}
