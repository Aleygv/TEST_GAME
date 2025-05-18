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

    private int levelOfBaitTEST = 2;

    // In case of random, this list becomes active in the Editor
    public List<Item> itemsToGive = new List<Item>();
    
    public List<BaitItem> baitsToGive = new List<BaitItem>();

    // The minimum number of a random item to be given, needs to be at least 1
    public int minimumItemsToGive = 1;

    // The maximum number of a random item to be given, needs to be at least 1
    public int maximumItemsToGive = 1;


    // Adds one random Item from the pre selected list to the Inventory
    //The quantity to be added is also random based on the minimumItemsToGive and maximumItemsToGiv
    public void AddRandomItem()
    {
        //Затычка в GenerateFish, нужно передавать потом от конкретной наживки
        Inventory.instance.AddItem(itemsToGive[FishGenerator.GenerateFish(levelOfBaitTEST) - 1], Random.Range(minimumItemsToGive, maximumItemsToGive));
    }

}
