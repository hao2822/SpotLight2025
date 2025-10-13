using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemBase
{
    public string Name;
    public ItemAging Aging;
    //public ItemOrigin Origin;
    public ItemMaterial Material;
    public ItemCulture Culture;
    public List<KeyCode> KeyCodes;
    public uint Price;

    public bool EnableAdd;
    public bool EnableRemove;

    public Sprite Icon;
    public Action OnStartRemove;
    public Action OnCancelRemove;
    public static ItemBase RandomNew(ItemRare rare = ItemRare.Common)
    {
        var item = new ItemBase();
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            item.Name = AuctionItemDictionary.AuctionItems.Keys.ToList()[Random.Range(0, AuctionItemDictionary.AuctionItems.Count)];
        }
        else
        {
            item.Name = AuctionItemDictionary.AuctionItems.Values.ToList()[Random.Range(0, AuctionItemDictionary.AuctionItems.Count)];
            
        }
        item.Aging = (ItemAging)Random.Range(0, System.Enum.GetNames(typeof(ItemAging)).Length);
        //item.Origin = (ItemOrigin)Random.Range(0, System.Enum.GetNames(typeof(ItemOrigin)).Length);
        item.Material = (ItemMaterial)Random.Range(0, System.Enum.GetNames(typeof(ItemMaterial)).Length);
        item.Culture = (ItemCulture)Random.Range(0, System.Enum.GetNames(typeof(ItemCulture)).Length);
        item.KeyCodes = new List<KeyCode>();
        switch (rare)
        {
            case ItemRare.Common:
                item.Price = (uint)Random.Range(5000, 10000);
                break;
            case ItemRare.Rare:
                item.Price = (uint)Random.Range(10000, 20000);
                break;
            case ItemRare.Epic:
                item.Price = (uint)Random.Range(20000, 50000);
                break;
            case ItemRare.Legendary:
                item.Price = (uint)Random.Range(50000, 100000);
                break;
            
        }
        var index = Random.Range(1, 22);
        var id = index < 10 ? $"0{index}" : $"{index}";
        var fileName= $"obj{id}";
        item.Icon = Resources.Load<Sprite>($"Obj/{fileName}");
        //Debug.Log(item);
        return item;
    }

    public static ItemBase New(Consignor consignor)
    {
        var item = new ItemBase();
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            item.Name = AuctionItemDictionary.AuctionItems.Keys.ToList()[Random.Range(0, AuctionItemDictionary.AuctionItems.Count)];
        }
        else
        {
            item.Name = AuctionItemDictionary.AuctionItems.Values.ToList()[Random.Range(0, AuctionItemDictionary.AuctionItems.Count)];
            
        }
        var index = Random.Range(1, 22);
        var id = index < 10 ? $"0{index}" : $"{index}";
        var fileName= $"obj{id}";
        item.Icon = Resources.Load<Sprite>($"Obj/{fileName}");
        item.Aging = consignor.Aging;
        //item.Origin = consignor.Origin;
        item.Material = consignor.Material;
        item.Culture = consignor.Culture;
        item.KeyCodes = new List<KeyCode>();
        item.Price = (uint)Random.Range(50000, 100000);
        //Debug.Log(item);
        return item;
    }
    public override string ToString()
    {
        return $"【{Name}】 {Aging} {Material} {Culture} [{string.Join(",", KeyCodes)}]";
    }
}

public enum ItemRare
{
    Common,
    Rare,
    Epic,
    Legendary,
}
