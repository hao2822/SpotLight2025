using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [ExecuteInEditMode]
    public class UIKeys : SerializedMonoBehaviour
    {
        public KeyCode Key;
        public ItemBase MyItem;
        public Text KeyCodeText;
        public bool Pressed;

        static List<ItemBase> ReadyToRemoveItems = new List<ItemBase>();

        Tweener tweener;
        
        public Image ActiveImage;

        private void OnEnable()
        {
            KeyCodeText.text = Key.ToString().Replace("Alpha", "");
            InputKeyManager.OnKeyPressed -= OnKeyPressed;

            InputKeyManager.OnKeyPressed += OnKeyPressed;
            ActiveImage.gameObject.SetActive(Pressed);
        }

        private void OnKeyPressed(List<KeyCode> keys)
        {
            //keys = InputKeyManager.allKeys.ToList();
            var image = GetComponent<Image>();
            bool pressed = keys.Contains(Key);
            image.color = pressed ? Color.red : Color.white;
            ActiveImage.gameObject.SetActive(pressed);
            if (pressed)
            {
                if (!Pressed)
                {
                    AudioManager.Instance.PlayDLG(GameAFX.Tap);
                    tweener?.Kill();
                    tweener = ActiveImage.GetComponent<RectTransform>().DOPunchScale(new Vector3(0, 0.5f, 0), 0.2f).OnComplete((
                        () =>
                        {
                            tweener?.Kill();
                            tweener = ActiveImage.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 5), 1f)
                                .From(new Vector3(0, 0, 0)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(0.2f);
                        }));
                }

                Pressed = true;
            }
            else
            {
                if (Pressed)
                {
                    tweener?.Kill();
                    tweener = ActiveImage.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0f);
                    if (MyItem != null)
                    {
                        if (GameManager.Instance.MyItems.Contains(MyItem) && !ReadyToRemoveItems.Contains(MyItem))
                        {
                            ReadyToRemoveItems.Add(MyItem);
                            Debug.Log($"开始移除物品 {MyItem.Name}");
                            MyItem.OnStartRemove?.Invoke();
                            CoroutineUtil.Instance.StartCoroutine(RemoveItem(MyItem), MyItem.Name);
                        }
                    }
                }

                Pressed = false;
            }


            if (MyItem != null && AllPressed(keys))
            {
                if (!GameManager.Instance.MyItems.Contains(MyItem) && MyItem.EnableAdd)
                {
                    AudioManager.Instance.PlayAFX(GameAFX.Get);
                    GameManager.Instance.MyItems.Add(MyItem);
                    GameManager.OnMyItemChanged?.Invoke();
                }

                if (ReadyToRemoveItems.Contains(MyItem))
                {
                    Debug.Log($"物品 {MyItem.Name} 取消移除");
                    MyItem.OnCancelRemove?.Invoke();
                    ReadyToRemoveItems.Remove(MyItem);
                }


                CoroutineUtil.Instance.StopCoroutine(MyItem.Name);
            }
        }

        IEnumerator RemoveItem(ItemBase item)
        {
            yield return new WaitForSeconds(5f);

            if (ReadyToRemoveItems.Contains(item))
            {
                ReadyToRemoveItems.Remove(item);
            }

            if (item.EnableRemove)
            {
                Debug.Log($"移除 {item.Name}");
                GameManager.Instance.MyItems.Remove(item);
                AudioManager.Instance.PlayAFX(GameAFX.Lost);
            }

            GameManager.OnMyItemChanged?.Invoke();
        }

        bool AllPressed(List<KeyCode> keys)
        {
            foreach (var key in MyItem.KeyCodes)
            {
                if (!keys.Contains(key))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnDisable()
        {
            InputKeyManager.OnKeyPressed -= OnKeyPressed;
        }
    }
}