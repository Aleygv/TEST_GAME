using System;
using UnityEngine;
using System.Collections.Generic;

public class SellZone : MonoBehaviour
{
    [SerializeField] private List<FishItem> fishItems; // Список всех видов рыб
    [SerializeField] private PlayerMoneyUI playerMoneyUI;
    
    public void SellAllFish()
    {
        int totalFishSold = 0;
        int totalMoneyEarned = 0;

        var items = Inventory.instance.GetAllItems();

        foreach (var item in new List<Item>(items))
        {
            if (item is FishItem fishItem)
            {
                // Находим ScriptableObject для этой рыбы
                FishItem fishSO = fishItems.Find(f => f == fishItem);
                if (fishSO != null)
                {
                    int count = Inventory.instance.GetItemCount(item);
                    if (count > 0)
                    {
                        Inventory.instance.RemoveItem(item, item.itemType, count);
                        int earned = count * fishSO.price;
                        totalFishSold += count;
                        totalMoneyEarned += earned;
                        
                    }
                }
            }
        }

        if (totalFishSold > 0)
        {
            PlayerMoney.Instance.AddMoney(totalMoneyEarned);
            playerMoneyUI.UpdateMoney(PlayerMoney.Instance.Value);
            Debug.Log($"Продано рыбы: {totalFishSold}, заработано: {totalMoneyEarned}");
        }
        else
        {
            GameManager.ShowHasFishToSellMessage();
            Debug.Log("Нет рыбы для продажи.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // _interactButton.SetActive(true);
        if (other.gameObject.CompareTag("Player"))
        {
            SellAllFish();
        }
    }
}