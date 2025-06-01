using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// IN THIS SCRIPT: Inventory Main Script that handles items and respective quantities aqquired
// WARNING: This script uses UNITY Editor to simplify the process of setting it up
// USE THIS SCRIPT by attaching it to any GameObject(Ex. PlayerPrefab, EmptyObject)


public enum ItemType
{
    Fish,
    Bait,
    Note
}


public class Inventory : MonoBehaviour
{
    // // The items on the inventory
    // public List<Item> itemList = new List<Item>();
    //
    // // The correponding quantities of each item
    // public List<int> quantityList = new List<int>();

    // Вместо одного inventoryPanel будет несколько
    [SerializeField] private Transform fishSlotParent;
    [SerializeField] private Transform baitSlotParent;
    [SerializeField] private Transform noteSlotParent;

    // Списки слотов для каждого типа
    private List<InventorySlot> fishSlots = new List<InventorySlot>();
    private List<InventorySlot> baitSlots = new List<InventorySlot>();
    private List<InventorySlot> noteSlots = new List<InventorySlot>();

    // Используем Dictionary как раньше
    public Dictionary<ItemType, List<Item>> itemLists = new Dictionary<ItemType, List<Item>>();
    public Dictionary<ItemType, List<int>> quantityLists = new Dictionary<ItemType, List<int>>();

    // The inventoryPanel is the parent object of each slot
    public GameObject inventoryPanel;

    // The slotList is the list of slots on the inventory, you can turn this List public and place the slots manually inside of it
    // Currently it's making the list based on the inventoryPanel children objects on GatherSlots() in line 86
    List<InventorySlot> slotList = new List<InventorySlot>();


    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;
        
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            itemLists[type] = new List<Item>();
            quantityLists[type] = new List<int>();
        }
        
        GatherSlots();
    }

    #endregion


    public void Start()
    {
        // Add the slots of the Inventory Panel to the list

        foreach (InventorySlot child in inventoryPanel.GetComponentsInChildren<InventorySlot>())
        {
            slotList.Add(child);
        }
        
        UpdateInventoryUI();
    }
    
    private void GatherSlots()
    {
        fishSlots.Clear();
        baitSlots.Clear();
        noteSlots.Clear();

        foreach (InventorySlot slot in fishSlotParent.GetComponentsInChildren<InventorySlot>())
        {
            fishSlots.Add(slot);
        }
        foreach (InventorySlot slot in baitSlotParent.GetComponentsInChildren<InventorySlot>())
        {
            baitSlots.Add(slot);
        }
        foreach (InventorySlot slot in noteSlotParent.GetComponentsInChildren<InventorySlot>())
        {
            noteSlots.Add(slot);
        }
    }

    // AddItem() can be called in other scripts with the following line:
    //Inventory.instance.Add(ItemYouWantToGiveHere , quantityOfThatItem);
    // Currently it's being called by the AddItemToInventory Script on the Add Items Buttons 
    public void AddItem(Item itemAdded, int quantityAdded)
    {
        ItemType type = itemAdded.itemType;

        if (itemAdded.Stackable)
        {
            List<Item> items = itemLists[type];
            List<int> quantities = quantityLists[type];

            if (items.Contains(itemAdded))
            {
                quantities[items.IndexOf(itemAdded)] += quantityAdded;
            }
            else
            {
                int totalItemCount = TotalItemCount();
                if (totalItemCount < slotList.Count)
                {
                    items.Add(itemAdded);
                    quantities.Add(quantityAdded);
                }
                else
                {
                    Debug.LogWarning("Inventory full!");
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < quantityAdded; i++)
            {
                List<Item> items = itemLists[itemAdded.itemType];
                List<int> quantities = quantityLists[itemAdded.itemType];

                int totalItemCount = TotalItemCount();
                if (totalItemCount < slotList.Count)
                {
                    items.Add(itemAdded);
                    quantities.Add(1);
                }
                else
                {
                    Debug.LogWarning("Inventory full!");
                    return;
                }
            }
        }

        UpdateInventoryUI();
    }
    
    public int GetItemCount(Item item)
    {
        ItemType type = item.itemType;
        if (!itemLists.ContainsKey(type)) return 0;

        List<Item> items = itemLists[type];
        List<int> quantities = quantityLists[type];

        int count = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                count += quantities[i];
            }
        }
        return count;
    }

// Возвращает список всех предметов всех типов в инвентаре (без повторов)
    public List<Item> GetAllItems()
    {
        List<Item> allItems = new List<Item>();
        foreach (var pair in itemLists)
        {
            allItems.AddRange(pair.Value);
        }
        return allItems;
    }
    
    private int TotalItemCount()
    {
        int count = 0;
        foreach (var list in itemLists.Values)
        {
            count += list.Count;
        }
        return count;
    }

    // As the previous function, this can be called from another script
    // Currently called by the Remove Button in each InventorySlot Prefab
    public void RemoveItem(Item itemRemoved, ItemType type, int quantityRemoved)
    {
        List<Item> items = itemLists[type];
        List<int> quantities = quantityLists[type];

        if (!items.Contains(itemRemoved))
            return;

        int index = items.IndexOf(itemRemoved);

        if (itemRemoved.Stackable)
        {
            quantities[index] -= quantityRemoved;

            if (quantities[index] <= 0)
            {
                items.RemoveAt(index);
                quantities.RemoveAt(index);
            }
        }
        else
        {
            for (int i = 0; i < quantityRemoved && index < items.Count && items[index] == itemRemoved; i++)
            {
                items.RemoveAt(index);
                quantities.RemoveAt(index);
            }
        }

        UpdateInventoryUI();
    }
    
    public bool HasItem(Item item, ItemType type)
    {
        if (!itemLists.ContainsKey(type)) return false;

        List<Item> items = itemLists[type];
        return items.Contains(item);
    }


    // --------------------------------------------------UI------------------------------------------------


    // Everytime an item is Added or Removed from the Inventory, the UpdateInventoryUI runs
    public void UpdateInventoryUI()
    {
        UpdateSlotGroup(ItemType.Fish, fishSlots);
        UpdateSlotGroup(ItemType.Bait, baitSlots);
        UpdateSlotGroup(ItemType.Note, noteSlots);
    }

    private void UpdateSlotGroup(ItemType type, List<InventorySlot> slots)
    {
        List<Item> items = itemLists[type];
        List<int> quantities = quantityLists[type];

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].UpdateSlot(items[i], quantities[i], type, i);
            }
            else
            {
                slots[i].UpdateSlot(null, 0, type, -1); // Пустой слот
            }
        }
    }
}