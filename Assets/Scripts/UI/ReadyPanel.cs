using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPanel : MonoBehaviour
{
    public Text ReadyText;
    public RectTransform Image1;
    public RectTransform Image2;
    private void OnEnable()
    {
        Image1.DOAnchorPosY(900, 1f).From(new Vector2(0,368));
        Image2.DOAnchorPosY(-900, 1f).From(new Vector2(0,-368)).OnComplete(ToStart);
        
        
    }
    async void ToStart()
    {
        await UniTask.Delay(1000);
        GameManager.Instance.Starter = true;
        gameObject.SetActive(false);
    }
    
}
