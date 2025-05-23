using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectedItemManager : MonoBehaviour
{
    public static SelectedItemManager Instance { get; private set; }

    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemLevelText;

    public Item selectedItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        ClearSelectedItem();
    }

    public void SetSelectedItem(Item item)
    {
        selectedItem = item;

        if (item != null)
        {
            selectedItemIcon.enabled = true;
            selectedItemIcon.sprite = item.itemIcon;

            itemNameText.text = item.itemName; // Убедись, что поле description есть в Item  // Например int price
            
            // Проверяем, является ли предмет BaitItem
            if (item is BaitItem baitItem)
            {
                itemLevelText.gameObject.SetActive(true);
                itemLevelText.text = "Level " + baitItem.levelOfBait;
            }
            else
            {
                itemLevelText.gameObject.SetActive(false);
            }
        }
        else
        {
            ClearSelectedItem();
        }
    }

    public void ClearSelectedItem()
    {
        selectedItem = null;
        selectedItemIcon.enabled = false;
        itemNameText.text = "";
        itemLevelText.text = "";
    }
}