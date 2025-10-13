using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using I18N;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TurnSettlementPanel : MonoBehaviour
{
    public RectTransform ItemContainer;
    public GameObject ItemCellPrefab;

    List<UIItemCell> ItemCells = new List<UIItemCell>();
    public ToggleGroup ToggleGroup;
    Tweener tweener;
    public Text MoneyText;

    public Text ContentText;

    public Text SettlementButtonText;
    public Text NextButtonText;
    
    public Text AgingText;
    public Text MaterialText;
    public Text CultureText;


    public Text AddMoneyText;
    public Text GoalText;

    public Image BlackMask;
    public GameObject NextButton;
    public Image AvatarImage;
    public Text NameText;
    public void Show()
    {
        BlackMask.DOFade(0, 0f).From(0.5f).SetEase(Ease.Linear);
        NextButton.SetActive(true);
        tweener?.Kill();
        tweener = transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);
        SettlementButtonText.text = I18NConst.Settlement;
        NextButtonText.text = I18NConst.NextTurn;
        price = bonus = allBonus = 0;
        ContentText.text = string.Empty;
        gameObject.SetActive(true);
        MoneyText.text = GameManager.Instance.Money.ToString();

        GoalText.text = GameManager.Instance.GoalMoney.ToString();
        ItemCells.Clear();
        var itemCells = transform.GetComponentsInChildren<UIItemCell>();
        foreach (var itemCell in itemCells)
        {
            Destroy(itemCell.gameObject);
        }

        foreach (var myItem in GameManager.Instance.MyItems)
        {
            price += myItem.Price;
        }
        var c = GameManager.Instance.Consignors.FirstOrDefault();
        if (c != null)
        {
            AvatarImage.sprite = c.Avatar;
            NameText.text = c.Name;
        }
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

        AddMoneyText.text = $"+{price}";
        foreach (var item in GameManager.Instance.MyItems)
        {
            var itemCell = Instantiate(ItemCellPrefab, ItemContainer);
            var t = itemCell.GetComponent<UIItemCell>().SetToggle(item);
            t.group = ToggleGroup;
            ItemCells.Add(itemCell.GetComponent<UIItemCell>());
            t.onValueChanged.AddListener(OnValueChanged);
        }
    }

    private void OnValueChanged(bool v)
    {
        if (v)
        {
            var t = ToggleGroup.GetFirstActiveToggle().GetComponent<UIItemCell>();
            var item = t.Item;
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
        }
    }

    uint price = 0;
    uint bonus = 0;
    uint allBonus = 0;
    public void Settlement()
    {
        foreach (var cell in ItemCells)
        {
            cell.GetComponent<Toggle>().interactable = false;
        }

        ItemBase item = null;
        if (ToggleGroup.GetFirstActiveToggle() != null)
        {
            item = ToggleGroup.GetFirstActiveToggle().GetComponent<UIItemCell>().Item;
        }
        
        int i = 0;
        var c = GameManager.Instance.Consignors.FirstOrDefault();
        
        

        
        if (c != null && item != null)
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

        var content = string.Empty;
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            content = $"你获得了{price}元，获得了{bonus+allBonus}元来自{c?.Name}的打赏，共获得了{price + bonus + allBonus}元";
        }
        else
        {
            content =
                $"You received {price} yuan, received {bonus + allBonus} yuan in tips from {c?.Name}, making a total of {price + bonus + allBonus} yuan.";
        }
        ContentText.text = content;
    }

    public void Next()
    {
        tweener?.Kill();
        float myValue = GameManager.Instance.Money;
        if (price + bonus + allBonus != 0)
        {
            tweener = DOTween.To(() => myValue, x => myValue = x, GameManager.Instance.Money + price + bonus + allBonus, 2)
                .SetEase(Ease.Linear).OnUpdate(() => { MoneyText.text = ((int)(myValue)).ToString(); }).OnComplete(OnCompleted);
        }
        else
        {
            OnCompleted();
        }
        
    }

    async void OnCompleted()
    {
        GameManager.Instance.Money += (int)(price + bonus + allBonus);
        MoneyText.text = GameManager.Instance.Money.ToString();
        GameManager.Instance.BlackMask.DOFade(1, 0.8f).From(0).OnComplete(() =>
        {
            //BlackMask.gameObject.SetActive(false);
        });
        await UniTask.Delay(1000);
        gameObject.SetActive(false);
        GameManager.Instance.ToNextTurn();
    }
}