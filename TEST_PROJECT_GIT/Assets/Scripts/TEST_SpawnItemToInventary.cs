using System;
using UnityEngine;

public class TEST_SpawnItemToInventary : MonoBehaviour
{
    // The Specific item you want to add
    public Item specificItem;

    // The Specific quantity you want to add
    public int specificQuant;

    // Adds the specific item and quantity you set up in the Editor to the Inventory
    public void AddSpecificItem()
    {
        Inventory.instance.AddItem(specificItem, specificQuant);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        AddSpecificItem();
        
    }
}
