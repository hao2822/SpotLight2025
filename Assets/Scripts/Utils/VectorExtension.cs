using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {
        /// <summary>
        /// 判断两个 Vector3 是否近似相等
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <param name="threshold">比较的阈值，默认为 0.0001f</param>
        /// <returns>如果两个向量之间的距离小于阈值，则返回 true</returns>
        public static bool IsApproximatelyEqual(this Vector3 v1, Vector3 v2, float threshold = 0.01f)
        {
            return (v1 - v2).sqrMagnitude < threshold;
        }


        /// <summary>
        /// 判断两个 Vector2 是否近似相等
        /// </summary>
        /// <param name="v1">第一个向量</param>
        /// <param name="v2">第二个向量</param>
        /// <param name="threshold">比较的阈值，默认为 0.0001f</param>
        /// <returns>如果两个向量之间的距离小于阈值，则返回 true</returns>
        public static bool IsApproximatelyEqual(this Vector2 v1, Vector2 v2, float threshold = 0.01f)
        {
            return (v1 - v2).sqrMagnitude < threshold;
        }
    }
}
