using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// 单例模式
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T Reset()
        {
            _instance = new T();
            return _instance;
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }

    /// <summary>
    /// MonoBehaviour的单例模式
    /// </summary>
    public class SingletonBehaviour<T> : SerializedMonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T instance;
        public static bool isExist => instance != null;
        public static T Reset()
        {
            instance = (T)FindAnyObjectByType(typeof(T));
            return instance;
        }

        protected virtual void Initialize()
        {
        }

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (T)FindAnyObjectByType(typeof(T));
                if(instance == null)
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                instance.Initialize();
                return instance;
            }
        }
    }
}