using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneUtil
    {
        public static Action<float> OnLoadAction;
        public static void LoadSceneAsync(string name, Action callback = null,float delay = 2)
        {
            CoroutineUtil.Instance.StartCoroutine(InternalLoadScene(name, callback ,delay),
                $"{nameof(InternalLoadScene)}_{name}");
        }
        static IEnumerator InternalLoadScene(string name,Action callback,float delay)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                OnLoadAction?.Invoke(asyncOperation.progress);
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(delay);
                    asyncOperation.allowSceneActivation = true;
                    
                    callback?.Invoke();
                    OnLoadAction = null;
                    break;
                }

                yield return null;
            }
        }
    }
}
