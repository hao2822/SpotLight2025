using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using I18N;
using UnityEngine;
using UnityEngine.UI;

public class SelectConsignorPanel : MonoBehaviour
{
    public Text GoalText;
    public Text MoneyText;
    public Text AcceptButtonText;
    public Text RefuseButtonText;
    public Text ConsignorNameText;
    Consignor Consignor;
    public Text AgingText;
    public Text MaterialText;
    public Text CultureText;
    public Image BlackMask;
    public Image AvatarImage;

    public Text DateText;
    private void OnEnable()
    {
        GameManager.Instance.BlackMask.DOFade(0, 2f).From(1).OnComplete(() =>
        {
            //BlackMask.gameObject.SetActive(false);
        });
    }

    public void Show(int consignorID)
    {
        gameObject.SetActive(true);
        Consignor = null;
        AcceptButtonText.text = I18NConst.Accept;
        RefuseButtonText.text = I18NConst.Refuse;
        DateText.text = $"Day{GameManager.Instance.CurrentTurn + 1}";
        if (consignorID < GameManager.Instance.TempConsignors.Count)
        {
            var c = GameManager.Instance.TempConsignors[consignorID];
            Consignor = c;
            GoalText.text = GameManager.Instance.GoalMoney.ToString();
            MoneyText.text = $"{GameManager.Instance.Money}";
            ConsignorNameText.text = c.Name;
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
            AvatarImage.sprite = c.Avatar;
        }
    }

    public void Next()
    {
        if (GameManager.Instance.TempConsignorIndex + 1 < GameManager.Instance.TempConsignors.Count)
        {
            GameManager.Instance.TempConsignorIndex++;
        }
        else
        {
            GameManager.Instance.TempConsignorIndex = 0;
        }
        Show(GameManager.Instance.TempConsignorIndex);
    }

    public void Accept()
    {
        if (Consignor != null)
        {
            GameManager.Instance.Consignors.Add(Consignor);
            gameObject.SetActive(false);
        }
    }
}