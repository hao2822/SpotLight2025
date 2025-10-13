using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinPanel : MonoBehaviour
{
    public RectTransform ItemContainer;
    public GameObject ItemCellPrefab;

    List<UIItemCell> ItemCells = new List<UIItemCell>();
    public Text ContentText;
    public void Show()
    {
        gameObject.SetActive(true);
        ItemCells.Clear();
        var itemCells = transform.GetComponentsInChildren<UIItemCell>();
        foreach (var itemCell in itemCells)
        {
            Destroy(itemCell.gameObject);
        }

        foreach (var item in GameManager.Instance.AllMyTurnItems)
        {
            var itemCell = Instantiate(ItemCellPrefab, ItemContainer);
            var t = itemCell.GetComponent<UIItemCell>().SetToggle(item);
            t.interactable = false;
            itemCell.GetComponent<UIItemCell>().KeysText.gameObject.SetActive(false);
            ItemCells.Add(itemCell.GetComponent<UIItemCell>());
        }
        ContentText.text = GameManager.Instance.CurrentI18N == I18N.I18N.CN ? $"×îÖÕµÃ·Ö£º{GameManager.Instance.Money}" : $"Final Score£º{GameManager.Instance.Money}";
    }
}
