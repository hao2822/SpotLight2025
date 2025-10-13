using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : SerializedMonoBehaviour
{
    public Text Name;

    public Text Tag;
    public Text PriceText;
    public Text KeysText;
    public Text TimeText;
    Tweener tweener;

    ItemBase Item;

    public Image Bar;
    
    public Text AgingText;
    public Text MaterialText;
    public Text CultureText;
    public Text ProgressText;
    public Image IconImage;

    public void Init(ItemBase item)
    {
        Name.text = item.Name;
        Item = item;
        GameManager.OnMyItemChanged -= OnMyItemChanged;
        GameManager.OnMyItemChanged += OnMyItemChanged;
        GameManager.OnTurnItemChanged -= OnTurnItemChanged;
        GameManager.OnTurnItemChanged += OnTurnItemChanged;
        OnMyItemChanged();
        IconImage.sprite = item.Icon;
        IconImage.SetNativeSize();
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            AgingText.text = $"{Item.Aging}";
            MaterialText.text = $"{Item.Material}";
            CultureText.text = $"{Item.Culture}";
        }
        else
        {
            AgingText.text = ItemDescriptionDictionaries.CombinedDescriptions[Item.Aging.ToString()];
            MaterialText.text = ItemDescriptionDictionaries.CombinedDescriptions[Item.Material.ToString()];
            CultureText.text = ItemDescriptionDictionaries.CombinedDescriptions[Item.Culture.ToString()];
        }
        ProgressText.text = $"{GameManager.Instance.CurrentItemIndex + 1}/{GameManager.Instance.AllItems.Count}";
        //Tag.text = sb.ToString();
        PriceText.text = "$" + item.Price.ToString();
        var p = GameManager.Instance.CurrentI18N == I18N.I18N.CN ? "按下：" : "HOLD:";
        KeysText.text = $"{p}{string.Join(",", item.KeyCodes).Replace("Alpha", "")}";
        tweener?.Kill();
        // float myValue = 5f;
        // tweener = DOTween.To(() => myValue, x => myValue = x, 0, 5).SetEase(Ease.Linear).OnUpdate(() =>
        // {
        //     TimeText.text = myValue.ToString("0.0");
        // });
        Bar.DOFillAmount(0, 5f).From(1).SetEase(Ease.Linear);
    }

    private void OnTurnItemChanged(ItemBase obj)
    {
        ProgressText.text = $"{GameManager.Instance.CurrentItemIndex + 1}/{GameManager.Instance.AllItems.Count}";
    }

    private void OnMyItemChanged()
    {
        var image = GetComponent<Image>();
        // if (GameManager.Instance.MyItems.Contains(Item))
        // {
        //     image.color = Color.green;
        // }
        // else
        // {
        //     image.color = Color.white;
        // }
    }

    private void OnDisable()
    {
        GameManager.OnTurnItemChanged -= OnTurnItemChanged;
        GameManager.OnMyItemChanged -= OnMyItemChanged;
    }
}