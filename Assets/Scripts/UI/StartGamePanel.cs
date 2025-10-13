using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class StartGamePanel : MonoBehaviour
{
    public Text StartGameButtonText;
    public Text EndGameButtonText;

    public Text TitleText;
    private void OnEnable()
    {
        GameManager.Instance.SetI18N(true);
        Refresh();

        GameManager.Instance.OnI18NChanged += Refresh;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnI18NChanged -= Refresh;
    }

    public void Exit()
    {
        ApplicationUtil.ExitGame();
    }
    void Refresh()
    {
        if (GameManager.Instance.CurrentI18N == I18N.I18N.CN)
        {
            StartGameButtonText.text = "开始游戏";
            EndGameButtonText.text = "结束游戏";
            TitleText.text = "逆转财拍";
        }
        else
        {
            StartGameButtonText.text = "Start";
            EndGameButtonText.text = "End";
            TitleText.text = "Buy IT";
        }

        
    }
}