using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
[ExecuteInEditMode]
public class UIKeysFormat : MonoBehaviour
{
    public float Delta;
    private void OnEnable()
    {
        var keys = GetComponentsInChildren<UIKeys>();
        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            key.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(key.GetComponent<RectTransform>().anchoredPosition.x, -50 + Delta * i);
        }
    }
}
