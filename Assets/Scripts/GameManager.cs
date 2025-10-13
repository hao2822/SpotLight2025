using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NodeCanvas.DialogueTrees;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Dictionary<KeyCode,UIKeys> UIKeysMap = new Dictionary<KeyCode, UIKeys>();
    public List<Consignor> Consignors = new List<Consignor>();
    List<KeyCode> KeyPool = new List<KeyCode>();
    
    public List<ItemBase> AllItems = new List<ItemBase>();

    public int GoalMoney = 0;
    private int EasyCount = 8;
    private int MediumCount = 5;
    private int HardCount = 2;
        
    // Start is called before the first frame update
    public int CurrentItemIndex = 0;
    public int CurrentTurn = 0;
    public List<ItemBase> MyItems = new List<ItemBase>();
    public List<ItemBase> AllMyTurnItems = new List<ItemBase>();
    public static Action OnMyItemChanged;
    public static Action OnTurnStarted;
    public static Action<ItemBase> OnTurnItemChanged;
    public ItemInfoPanel InfoPanel;
    bool NextTurn = false;
    public GameObject StartGamePanel;
    public TurnSettlementPanel TurnSettlementPanel;

    public int Money;
    
    public FinalScorePanel FinalScorePanel;
    public WinPanel WinPanel;

    public I18N.I18N CurrentI18N = I18N.I18N.EN;
    public Action OnI18NChanged;

    public AudioSource TapSource;
    
    public List<Consignor> TempConsignors = new List<Consignor>();
    public int TempConsignorIndex = 0;
    public SelectConsignorPanel  SelectConsignorPanel;
    public ReadyPanel ReadyPanel;
    
    public Dictionary<I18N.I18N,DialogueTreeController> DialogueTreeControllers = new Dictionary<I18N.I18N,DialogueTreeController>();
    
    public Image BlackMask;
    private void Start()
    {
        AudioManager.Instance.Init(); StartGamePanel.gameObject.SetActive(true);
        TurnSettlementPanel.gameObject.SetActive(false);
        FinalScorePanel.gameObject.SetActive(false);
        AudioManager.Instance.Play(GameBGM.TitleScreen,true);

    }

    public void PlayTapSound()
    {
        TapSource.Play();
    }
    
    public void SetI18N(bool v)
    {
        if (v)
        {
            CurrentI18N = I18N.I18N.CN;
        }
        else
        {
            CurrentI18N = I18N.I18N.EN;
        }
        Debug.Log(CurrentI18N);
        OnI18NChanged?.Invoke();
    }

    public void ToNextTurn()
    {
        NextTurn = true;
    }
    public void LaunchGame()
    {
        Money = 0;
        AllMyTurnItems?.Clear();
        CurrentTurn = 0;
        CurrentItemIndex = 0;
        NextTurn = true;
        StartGame().Forget();
    }

    public bool Starter;
    public async UniTask StartGame()
    {
        for (int i = 0; i < 6; i++)
        {
            GoalMoney += Random.Range(50000, 100000);
            await UniTask.WaitUntil(() => NextTurn);
            MyItems?.Clear();
            NextTurn = false;
            Debug.Log($"第{i}天开始");
            GetAllUIKeys();
            Consignors?.Clear();
            GenTempConsignor();
            await UniTask.WaitUntil(() => Consignors.Count > 0);
            
            GenAllItems();
            ReadyPanel.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => Starter);
            Starter  = false;
            await StartTurn();
            if (i == 5)
            {
                WinPanel.Show();
            }
        }
        FinalScorePanel.Show();
    }
    // Update is called once per frame
    

    async UniTask StartTurn()
    {
        CurrentTurn++;
        OnTurnStarted?.Invoke();
        for (int i = 0; i < AllItems.Count; i++)
        {
            AudioManager.Instance.PlayAFX(GameAFX.Turn);
            Debug.Log($"第{i}个物品");
            CurrentItemIndex = i;
            var currentItem = AllItems[i];
            InfoPanel.Init(currentItem);
            currentItem.EnableAdd = true;
            currentItem.EnableRemove = true;
            OnTurnItemChanged?.Invoke(currentItem);
            await UniTask.Delay(5000);
            currentItem.EnableAdd = false;
        }

        foreach (var item in MyItems)
        {
            item.EnableRemove = false;
        }
        AllMyTurnItems.AddRange(MyItems);
        TurnSettlementPanel.Show();
    }
    void GetAllUIKeys()
    {
        var uiKeys = FindObjectsOfType<UIKeys>();
        foreach (var key in uiKeys)
        {
            UIKeysMap[key.Key] = key;
        }
    }
    bool tutorialed = false;
    void GenTempConsignor()
    {
        TempConsignors?.Clear();
        TempConsignorIndex = 0;
        for (int i = 0; i < 8; i++)
        {
            TempConsignors?.Add(Consignor.New());
        }
        SelectConsignorPanel.Show(TempConsignorIndex);
        if (!tutorialed)
        {
            tutorialed = true;
            DialogueTreeControllers[CurrentI18N].StartDialogue();
        }
        
    }
    void GenAllItems()
    {
        AllItems?.Clear();
        KeyPool?.Clear();
        KeyPool = InputKeyManager.allKeys.ToList();
        var easyPool = new List<ItemBase>();
        var mediumPool = new List<ItemBase>();
        var hardPool = new List<ItemBase>();
        for (int i = 0; i < EasyCount; i++)
        {
            var r = Random.Range(0, 2);
            var item = ItemBase.RandomNew((ItemRare)r);
            for (int j = 0; j < 1; j++)
            {
                var key = KeyPool[Random.Range(0, KeyPool.Count)];
                item.KeyCodes.Add(key);
                KeyPool.Remove(key);
            }
            easyPool.Add(item);
        }
        for (int i = 0; i < MediumCount; i++)
        {
            var r = Random.Range(1, 3);
            var item = ItemBase.RandomNew((ItemRare)r);
            for (int j = 0; j < 2; j++)
            {
                var key = KeyPool[Random.Range(0, KeyPool.Count)];
                item.KeyCodes.Add(key);
                KeyPool.Remove(key);
            }
            mediumPool.Add(item);
        }
        for (int i = 0; i < HardCount - 1; i++)
        {
            var r = Random.Range(2, 4);
            var item = ItemBase.RandomNew((ItemRare)r);
            for (int j = 0; j < 3; j++)
            {
                var key = KeyPool[Random.Range(0, KeyPool.Count)];
                item.KeyCodes.Add(key);
                KeyPool.Remove(key);
            }
            hardPool.Add(item);
        }

        var targetItem = ItemBase.New(Consignors.FirstOrDefault());
        for (int j = 0; j < 3; j++)
        {
            var key = KeyPool[Random.Range(0, KeyPool.Count)];
            targetItem.KeyCodes.Add(key);
            KeyPool.Remove(key);
        }
        hardPool.Add(targetItem);
        
        AllItems.AddRange(easyPool);
        AllItems.AddRange(mediumPool);
        AllItems.AddRange(hardPool);
        ShuffleList(AllItems);
        var sb = new StringBuilder();
        foreach (var item in AllItems)
        {
            sb.AppendLine(item.ToString());
            foreach (var key in item.KeyCodes)
            {
                if (UIKeysMap.TryGetValue(key, out UIKeys uiKeys))
                {
                    uiKeys.MyItem = item;
                }
                else
                {
                    Debug.LogError($"Key {key} not found in UIKeys");
                }
            }
        }
        Debug.Log(sb);
    }

    public void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
