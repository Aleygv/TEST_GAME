using System;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private PlayerMovement _player;

    [SerializeField] private FishingSystem _fishingSystem;
    [SerializeField] private GameObject _interactButton;
    
    public bool IsPlayerInZone { get; private set; }
    public FishingSystem FishingSystem => _fishingSystem;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerMovement>();
        _sprite = transform.GetComponent<SpriteRenderer>();
        IsPlayerInZone = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerInZone = true;
            _interactButton.SetActive(true);
            _sprite.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerInZone = false;
            _sprite.color = Color.yellow;
            _interactButton.SetActive(false);
        }
    }

    public bool CanStartFishing()
    {
        if (!IsPlayerInZone) return false;
        
        // Проверяем, есть ли выбранная наживка
        Item selectedItem = SelectedItemManager.Instance.selectedItem;
        
        if (selectedItem != null && selectedItem is BaitItem baitItem)
        {
            // Проверяем, есть ли такая наживка в инвентаре
            return Inventory.instance.HasItem(baitItem, baitItem.itemType);
        }
        
        Debug.Log("Выберите наживку перед рыбалкой.");
        GameManager.ShowNoBaitMessage();
        return false;
    }
}