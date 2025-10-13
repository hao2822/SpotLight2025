using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector] public int remainingUses;   // 剩余次数
    protected PlayerPlatformer player;          // 快速访问玩家

    public void Init(PlayerPlatformer p, int maxUse)
    {
        player = p;
        remainingUses = maxUse;
    }

    // 外部调用：次数>0 就执行并减1
    public bool TryUse()
    {
        if (remainingUses <= 0) return false;
        DoSkill();
        remainingUses--;
        return true;
    }

    public abstract void DoSkill();
}