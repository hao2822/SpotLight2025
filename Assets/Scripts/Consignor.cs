using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Consignor
{
    public string Name;
    public uint AllComparedBonus;
    public uint EachTagComparedBonus;
    public ItemAging Aging;
    public ItemOrigin Origin;
    public ItemMaterial Material;
    public ItemCulture Culture;
    public Sprite Avatar;
    public Sprite AvatarPreview;

    public static Consignor New()
    {
        var consignor = new Consignor();
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            consignor.Name = CommonNames.Values.ToList()[Random.Range(0, CommonNames.Values.Count)];
            
        }
        else
        {
            consignor.Name = CommonNames.Keys.ToList()[Random.Range(0, CommonNames.Keys.Count)];
            
        }
        consignor.AllComparedBonus = (uint)Random.Range(5000, 10000);
        consignor.EachTagComparedBonus = (uint)Random.Range(15000, 20000);
        consignor.Aging = (ItemAging)Random.Range(0, System.Enum.GetNames(typeof(ItemAging)).Length);
        consignor.Origin = (ItemOrigin)Random.Range(0, System.Enum.GetNames(typeof(ItemOrigin)).Length);
        consignor.Material = (ItemMaterial)Random.Range(0, System.Enum.GetNames(typeof(ItemMaterial)).Length);
        consignor.Culture = (ItemCulture)Random.Range(0, System.Enum.GetNames(typeof(ItemCulture)).Length);
        var index = Random.Range(1, 7);
        var id = index < 8 ? $"0{index}" : $"{index}";
        var fileName= $"char{id}";
        consignor.Avatar = Resources.Load<Sprite>($"Char/{fileName}");
        consignor.AvatarPreview = Resources.Load<Sprite>($"Char/{fileName}_fix");
        return consignor;
    }
    public static Dictionary<string, string> CommonNames = new Dictionary<string, string>
    {
        // 男性名字
        { "James", "詹姆斯" },
        { "John", "约翰" },
        { "Robert", "罗伯特" },
        { "Michael", "迈克尔" },
        { "William", "威廉" },
        { "David", "大卫" },
        { "Richard", "理查德" },
        { "Charles", "查尔斯" },
        { "Joseph", "约瑟夫" },
        { "Thomas", "托马斯" },
        
        // 女性名字
        { "Mary", "玛丽" },
        { "Patricia", "帕特里夏" },
        { "Jennifer", "詹妮弗" },
        { "Linda", "琳达" },
        { "Elizabeth", "伊丽莎白" },
        { "Barbara", "芭芭拉" },
        { "Susan", "苏珊" },
        { "Jessica", "杰西卡" },
        { "Sarah", "莎拉" },
        { "Karen", "卡伦" }
    };

    public override string ToString()
    {
        return $"【{Name}】 全部匹配：{AllComparedBonus} 每个标签匹配：{EachTagComparedBonus} 需求： {Aging} {Origin} {Material} {Culture}";
    }
}