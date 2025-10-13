using System.Collections.Generic;

public class ItemDescriptionDictionaries
{
    public static Dictionary<string, string> CombinedDescriptions = new Dictionary<string, string>();

    static ItemDescriptionDictionaries()
    {
        // 合并老化程度
        foreach (var item in AgingDescriptions)
        {
            CombinedDescriptions.Add(item.Key.ToString(), item.Value);
        }

        // 合并产地
        foreach (var item in OriginDescriptions)
        {
            CombinedDescriptions.Add(item.Key.ToString(), item.Value);
        }

        // 合并材质
        foreach (var item in MaterialDescriptions)
        {
            CombinedDescriptions.Add(item.Key.ToString(), item.Value);
        }

        // 合并文化底蕴
        foreach (var item in CultureDescriptions)
        {
            CombinedDescriptions.Add(item.Key.ToString(), item.Value);
        }
    }

    public static Dictionary<ItemAging, string> AgingDescriptions = new Dictionary<ItemAging, string>
    {
        { ItemAging.古色古香, "Antique charm" },
        { ItemAging.饱经风霜, "Exquisite" },
        { ItemAging.沧桑斑驳, "Futuristic" },
        { ItemAging.温润如玉, "Vibrant Colors" },
        { ItemAging.陈旧但不失气韵, "Abstract" },
        { ItemAging.原始古朴, "Layered Aesthetic" },
        { ItemAging.保存完好, "Well-preserved" },
        { ItemAging.带有幽暗光泽, "The Tenth Art" },
        { ItemAging.见证岁月, "Expressionist" },
        //{ ItemAging.历史沉淀感, "With a sense of historical sedimentation" }
    };

    public static Dictionary<ItemOrigin, string> OriginDescriptions = new Dictionary<ItemOrigin, string>
    {
        { ItemOrigin.宫廷御制, "Imperial" },
        { ItemOrigin.名窑烧造, "Bold" },
        { ItemOrigin.西域风情, "Exotic style" },
        { ItemOrigin.欧陆古典, "Classicism" },
        { ItemOrigin.东瀛匠心, "Craftsmanship" },
        { ItemOrigin.源自名门, "Delicate" },
        { ItemOrigin.大师故里, "Fine Detail" },
        { ItemOrigin.秘境所出, "Mysterious" },
        { ItemOrigin.非洲土著, "Vintage" },
        { ItemOrigin.殖民时期, "Conceptual" }
    };

    public static Dictionary<ItemMaterial, string> MaterialDescriptions = new Dictionary<ItemMaterial, string>
    {
        { ItemMaterial.温润剔透, "Ethereal" },
        { ItemMaterial.坚实厚重, "Vivid" },
        { ItemMaterial.纹理天成, "Harmonious" },
        { ItemMaterial.金光璀璨, "Radiant" },
        { ItemMaterial.银光熠熠, "Enigmatic" },
        { ItemMaterial.晶莹润泽, "Avant-garde" },
        { ItemMaterial.细腻如玉, "Expressive" },
        { ItemMaterial.粗犷质朴, "Poetic" },
        { ItemMaterial.稀有珍贵, "Metaphorical" },
        { ItemMaterial.天外陨铁, "Transcendent" }
    };

    public static Dictionary<ItemCulture, string> CultureDescriptions = new Dictionary<ItemCulture, string>
    {
        { ItemCulture.底蕴深厚, "Volumetric" },
        { ItemCulture.承载文明, "Simplified" },
        { ItemCulture.艺术瑰宝, "Artistic treasure" },
        { ItemCulture.具有重要历史意义, "Historical" },
        { ItemCulture.宗教神圣感, "Religious sanctity" },
        //{ ItemCulture.文人雅趣, "Reflecting the taste of scholars and literati" },
        { ItemCulture.象征吉祥, "Auspiciousness" },
        { ItemCulture.传奇身世, "Legendary" },
        { ItemCulture.独具神韵, "Charm" },
        { ItemCulture.大师之作, "Masterpiece" }
    };
}

public enum ItemAging
{
    古色古香,
    饱经风霜,
    沧桑斑驳,
    温润如玉,
    陈旧但不失气韵,
    原始古朴,
    保存完好,
    带有幽暗光泽,
    见证岁月,
    //历史沉淀感
}

public enum ItemOrigin
{
    宫廷御制,
    名窑烧造,
    西域风情,
    欧陆古典,
    东瀛匠心,
    源自名门,
    大师故里,
    秘境所出,
    非洲土著,
    殖民时期
}

public enum ItemMaterial
{
    温润剔透,
    坚实厚重,
    纹理天成,
    金光璀璨,
    银光熠熠,
    晶莹润泽,
    细腻如玉,
    粗犷质朴,
    稀有珍贵,
    天外陨铁
}

public enum ItemCulture
{
    底蕴深厚,
    承载文明,
    艺术瑰宝,
    具有重要历史意义,
    宗教神圣感,
    //文人雅趣,
    象征吉祥,
    传奇身世,
    独具神韵,
    大师之作
}