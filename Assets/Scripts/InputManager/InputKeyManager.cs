using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class InputKeyManager : SingletonBehaviour<InputKeyManager>
{
    private List<KeyCode> pressedKeys = new List<KeyCode>();
    public static Action<List<KeyCode>> OnKeyPressed;
    // 预定义要检测的所有键
    public static KeyCode[] allKeys = new KeyCode[]
    {
        // 字母 A-Z
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z,
        
        KeyCode.Alpha3, KeyCode.Alpha6, KeyCode.Alpha9,
        // 数字 0-9
        // KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        // KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7,
        // KeyCode.Alpha8, KeyCode.Alpha9,
        
        // 小键盘数字 0-9
        // KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
        // KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7,
        // KeyCode.Keypad8, KeyCode.Keypad9
    };
    
    void Update()
    {
        pressedKeys.Clear();
        
        // 遍历所有预定义的键
        foreach (KeyCode key in allKeys)
        {
            if (Input.GetKey(key))
            {
                pressedKeys.Add(key);
            }
        }
        
        // 每帧显示按下的键（可选）
        //PrintPressedKeys();
        OnKeyPressed?.Invoke(pressedKeys);
    }
    
    void PrintPressedKeys()
    {
        if (pressedKeys.Count > 0)
        {
            string keysString = "当前按下的键: ";
            foreach (KeyCode key in pressedKeys)
            {
                keysString += key.ToString() + " ";
            }
            Debug.Log(keysString);
        }
    }
}
