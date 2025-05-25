using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// IN THIS SCRIPT: Use this script to a random item in random quantities to the Inventory
// WARNING: This script uses UNITY Editor to simplify the process of setting it up
// USE THIS SCRIPT by attaching it next to the script that calls the AddItem() and set up what you want to Add to the inventory in the Editor

public class AddRandomItemToInventory : MonoBehaviour
{
    private void OnEnable()
    {
        FishGenerator.OnFishCaught += AddRandomItem;
    }

    private void OnDisable()
    {
        FishGenerator.OnFishCaught -= AddRandomItem;
    }

    // In case of random, this list becomes active in the Editor
    public List<Item> itemsToGive = new List<Item>();

    // The minimum number of a random item to be given, needs to be at least 1
    public int minimumItemsToGive = 1;

    // The maximum number of a random item to be given, needs to be at least 1
    public int maximumItemsToGive = 1;


    // Adds one random Item from the pre selected list to the Inventory
    public void AddRandomItem()
    {
        // Получаем выбранный предмет
        Item selectedItem = SelectedItemManager.Instance.selectedItem;

        if (selectedItem != null)
        {
            // Проверяем, является ли предмет наживкой
            if (selectedItem is BaitItem baitItem)
            {
                int levelOfBait = baitItem.levelOfBait;

                // Генерируем рыбу на основе уровня наживки
                int fishIndex = FishGenerator.GenerateFish(levelOfBait);

                // Добавляем рыбу в инвентарь
                if (fishIndex > 0 && fishIndex <= itemsToGive.Count)
                {
                    Item fishItem = itemsToGive[fishIndex - 1];
                    int quantity = Random.Range(minimumItemsToGive, maximumItemsToGive);
                    Inventory.instance.AddItem(fishItem, quantity);

                    // 🐟 Рыба успешно поймана — проверяем шанс потери наживки

                    if (Random.value < 0.5f) // 50% шанс
                    {
                        // Удаляем одну единицу наживки
                        Inventory.instance.RemoveItem(baitItem, baitItem.itemType, 1);

                        // Опционально: обновляем UI просмотра предмета
                        SelectedItemManager.Instance.ClearSelectedItem();
                    }
                }
                else
                {
                    Debug.LogWarning("Нет рыбы для уровня наживки: " + levelOfBait);
                }
            }
            else
            {
                Debug.LogWarning("Выбранный предмет — не наживка.");
            }
        }
        else
        {
            Debug.LogWarning("Не выбрано ни одного предмета.");
        }
    }
}