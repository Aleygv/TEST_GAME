using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item item;
    private ItemType itemType;
    private int itemIndex = -1;

    [SerializeField] private Image itemImage;
    [SerializeField] private Text quantityText;

    // Метод вызывается при клике на слот
    public void OnItemSelected()
    {
        if (item != null)
        {
            SelectedItemManager.Instance.SetSelectedItem(item);
        }
    }

    public void UpdateSlot(Item itemInSlot, int quantityInSlot, ItemType type, int index)
    {
        item = itemInSlot;
        itemType = type;
        itemIndex = index;

        if (itemInSlot != null && quantityInSlot > 0)
        {
            itemImage.enabled = true;
            itemImage.sprite = itemInSlot.itemIcon;

            if (quantityInSlot > 1)
            {
                quantityText.enabled = true;
                quantityText.text = quantityInSlot.ToString();
            }
            else
            {
                quantityText.enabled = false;
            }
        }
        else
        {
            itemImage.enabled = false;
            quantityText.enabled = false;
        }
    }
}