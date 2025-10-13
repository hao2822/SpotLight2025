using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using I18N;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ConsignorPanel : SerializedMonoBehaviour
{
    public Text NameText;
    public Text ContentText;
    public Text AgingText;
    public Text MaterialText;
    public Text CultureText;
    public Image AvatarImage;
    public RectTransform ContentTransform;
    private void OnEnable()
    {
        Refresh();
        GameManager.OnTurnStarted += Refresh;
        GameManager.OnTurnItemChanged += OnTurnItemChanged;
    }

    private void OnTurnItemChanged(ItemBase item)
    {
        var c = GameManager.Instance.Consignors.FirstOrDefault();
        int i = 0;
        if (c != null)
        {
            if(c.Aging == item.Aging)
            {
                i++;
            }
            // if(c.Origin == item.Origin)
            // {
            //     i++;
            // }
            if(c.Material == item.Material)
            {
                i++;
            }
            if(c.Culture == item.Culture)
            {
                i++;
            }
        }

        string content = string.Empty;
        if (i > 2)
        {
            content = I18NConst.ConsignorPerfect;
        }
        else if (i > 1)
        {
            content = I18NConst.ConsignorGood;
        }
        else if (i > 0)
        {
            content = I18NConst.ConsignorNormal;
        }
        else
        {
            content = I18NConst.ConsignorBad;
        }
        ContentText.text = content;
        ContentTransform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack);
    }

    void Refresh()
    {
        NameText.text = GameManager.Instance.Consignors.FirstOrDefault()?.Name;
        var c = GameManager.Instance.Consignors.FirstOrDefault();
        if (c != null)
        {
            AvatarImage.sprite = c.AvatarPreview;
            if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
            {
                AgingText.text = $"{c.Aging}";
                MaterialText.text = $"{c.Material}";
                CultureText.text = $"{c.Culture}";
            }
            else
            {
                AgingText.text = ItemDescriptionDictionaries.CombinedDescriptions[c.Aging.ToString()];
                MaterialText.text = ItemDescriptionDictionaries.CombinedDescriptions[c.Material.ToString()];
                CultureText.text = ItemDescriptionDictionaries.CombinedDescriptions[c.Culture.ToString()];
            }
        }
        //ContentText.text = GameManager.Instance.Consignors.FirstOrDefault()?.ToString();
    }
    private void OnDisable()
    {
        GameManager.OnTurnStarted -= Refresh;
        GameManager.OnTurnItemChanged -= OnTurnItemChanged;
    }
}
