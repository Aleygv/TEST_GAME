using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopZone : MonoBehaviour
{
    [SerializeField] private List<BaitItem> baitItems;
    [SerializeField] private Transform cellsPanel;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private PlayerMoneyUI playerMoneyUI; // Добавьте это поле

    private void Start()
    {
        if (playerMoneyUI == null)
            playerMoneyUI = FindObjectOfType<PlayerMoneyUI>();

        for (int i = 0; i < baitItems.Count && i < cellsPanel.childCount; i++)
        {
            var bait = baitItems[i];
            var cell = cellsPanel.GetChild(i).gameObject;

            var text = cell.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = bait.itemName + " - " + bait.price + "$";

            var iconTransform = cell.transform.Find("icon");
            if (iconTransform != null)
            {
                var iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                    iconImage.sprite = bait.itemIcon;
            }
        }
    }

    public void BuyBaitByIndex(int index)
    {
        if (index < 0 || index >= baitItems.Count) return;
        var bait = baitItems[index];
        if (PlayerMoney.Instance.Value >= bait.price)
        {
            PlayerMoney.Instance.AddMoney(-bait.price);
            Inventory.instance.AddItem(bait, 1);
            playerMoneyUI.UpdateMoney(PlayerMoney.Instance.Value); // Обновляем UI
            Debug.Log("Куплено: " + bait.itemName);
        }
        else
        {
            Debug.Log("Недостаточно денег для покупки " + bait.itemName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shopWindow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shopWindow.SetActive(false);
        }
    }
}