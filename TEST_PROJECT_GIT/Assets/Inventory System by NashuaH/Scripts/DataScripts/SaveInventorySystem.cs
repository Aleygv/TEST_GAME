using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JsonUtility = UnityEngine.JsonUtility;
using PlayerPrefs = UnityEngine.PlayerPrefs;

// IN THIS SCRIPT: Use this to save the items and quantities in your Inventory for your next session
// USE THIS SCRIPT by attaching to any object

[System.Serializable]
public class InventoryDataStr
{
    public List<Item> weaponItems = new List<Item>();
    public List<int> weaponQuantities = new List<int>();

    public List<Item> potionItems = new List<Item>();
    public List<int> potionQuantities = new List<int>();

    public List<Item> questItemItems = new List<Item>();
    public List<int> questItemQuantities = new List<int>();
}

public class SaveInventorySystem : MonoBehaviour
{
    public void SaveInventory()
    {
        InventoryDataStr saveData = new InventoryDataStr();

        // Сохраняем каждый тип отдельно
        SaveByType(Inventory.instance.itemLists[ItemType.Fish],
                   Inventory.instance.quantityLists[ItemType.Fish],
                   saveData.weaponItems,
                   saveData.weaponQuantities);

        SaveByType(Inventory.instance.itemLists[ItemType.Bait],
                   Inventory.instance.quantityLists[ItemType.Bait],
                   saveData.potionItems,
                   saveData.potionQuantities);

        SaveByType(Inventory.instance.itemLists[ItemType.Note],
                   Inventory.instance.quantityLists[ItemType.Note],
                   saveData.questItemItems,
                   saveData.questItemQuantities);

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    private void SaveByType(List<Item> items, List<int> quantities, List<Item> targetItems, List<int> targetQuantities)
    {
        targetItems.Clear();
        targetQuantities.Clear();

        for (int i = 0; i < items.Count && i < quantities.Count; i++)
        {
            targetItems.Add(items[i]);
            targetQuantities.Add(quantities[i]);
        }
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            InventoryDataStr data = JsonUtility.FromJson<InventoryDataStr>(json);

            Inventory inventory = Inventory.instance;

            // Очищаем текущие списки
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                inventory.itemLists[type].Clear();
                inventory.quantityLists[type].Clear();
            }

            // Загружаем по типам
            LoadByType(data.weaponItems, data.weaponQuantities, ItemType.Fish);
            LoadByType(data.potionItems, data.potionQuantities, ItemType.Bait);
            LoadByType(data.questItemItems, data.questItemQuantities, ItemType.Note);

            inventory.UpdateInventoryUI();
        }
    }

    private void LoadByType(List<Item> items, List<int> quantities, ItemType type)
    {
        for (int i = 0; i < items.Count && i < quantities.Count; i++)
        {
            Item item = items[i];
            int quantity = quantities[i];

            if (item != null)
            {
                Inventory.instance.itemLists[type].Add(item);
                Inventory.instance.quantityLists[type].Add(quantity);
            }
        }
    }
}
