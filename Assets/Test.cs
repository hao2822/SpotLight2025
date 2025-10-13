using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            ItemBase.RandomNew();
        }
    }
}
