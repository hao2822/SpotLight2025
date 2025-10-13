using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("技能配置")]
    public List<SkillConfig> allSkills = new();   // 5 种全配表
    public Transform skillsContainer;             // 空节点，用来挂技能脚本
    public GameObject bubblePrefab;               // 技能 1 预制体

    [Header("使用次数")]
    public int defaultUses = 3;

    readonly List<SkillBase> ownedSkills = new(); // 本局随机到的 3 个
    int currentIndex = 0;                         // 1/2/3 对应 0/1/2
    void Update()   // ← 新增
    {
        // 1/2/3 切换
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchTo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchTo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchTo(2);

        // Q 使用
        if (Input.GetKeyDown(KeyCode.Q)) UseCurrentSkill();
    }

    void Start()
    {
        RandomPickSkills();
        RegisterHotKeys();
    }

    void RandomPickSkills()
    {
        // 随机 3 个不重复
        var shuffled = allSkills.OrderBy(x => Random.value).Take(3).ToList();
        for (int i = 0; i < 3; i++)
        {
            var cfg = shuffled[i];
            SkillBase instance = skillsContainer.gameObject.AddComponent(System.Type.GetType(cfg.className)) as SkillBase;
            instance.Init(GetComponent<PlayerPlatformer>(), defaultUses);
            ownedSkills.Add(instance);
        }
        // 默认选中第 1 个
        currentIndex = 0;
    }

    void RegisterHotKeys()
    {
        // 1/2/3 切换
        for (int i = 0; i < 3; i++)
        {
            int capture = i;
        }
        // Q 使用
    }

    void SwitchTo(int index)
    {
        currentIndex = index;
    }

    void UseCurrentSkill()
    {
        bool success = ownedSkills[currentIndex].TryUse();
        Debug.Log("技能已用完！" + success);
    }
}

[System.Serializable]
public class SkillConfig
{
    public string skillName;
    public string className;   // "SkillBubble" / "SkillResize" ...
    public Sprite icon;
}