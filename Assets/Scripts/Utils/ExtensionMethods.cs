public static class ExtensionMethods
{

    /// <summary>
    /// 将value从from1到to1的范围映射到from2到to2的范围
    /// 归一化: 首先将 value 在原始范围 [from1, to1] 中的位置归一化到 [0, 1] 范围内。
    /// 这是通过计算 (value - from1) / (to1 - from1) 实现的。
    /// 
    /// 重新映射: 然后，将归一化后的值乘以目标范围 [from2, to2] 的长度 (to2 - from2)，
    /// 并加上目标范围的起始值 from2，从而将值映射到目标范围。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <param name="from2"></param>
    /// <param name="to2"></param>
    /// <returns></returns>
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}