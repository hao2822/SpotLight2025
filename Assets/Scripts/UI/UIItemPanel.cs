using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIItemPanel : SerializedMonoBehaviour
{
    public GameObject ItemCellPrefab;

    List<UIItemCell> ItemCells = new List<UIItemCell>();
    private void OnEnable()
    {
        Refresh();
        GameManager.OnTurnStarted += Refresh;
        GameManager.OnTurnItemChanged += OnTurnItemChanged;
    }

    private void OnTurnItemChanged(ItemBase item)
    {
        for (int i = 0; i < GameManager.Instance.CurrentItemIndex; i++)
        {
            var itemCell = ItemCells[i];
            itemCell.gameObject.SetActive(true);
            itemCell.AddEvent();
        }
    }

    private void Refresh()
    {
        ItemCells.Clear();
        var itemCells = transform.GetComponentsInChildren<UIItemCell>();
        foreach (var itemCell in itemCells)
        {
            Destroy(itemCell.gameObject);
        }
        foreach (var item in GameManager.Instance.AllItems)
        {
            var itemCell = Instantiate(ItemCellPrefab, transform);
            itemCell.GetComponent<UIItemCell>().SetItem(item);
            ItemCells.Add(itemCell.GetComponent<UIItemCell>());
            itemCell.gameObject.SetActive(false);
            
        }
    }
    private void OnDisable()
    {
        GameManager.OnTurnStarted -= Refresh;
        GameManager.OnTurnItemChanged -= OnTurnItemChanged;
    }
}
