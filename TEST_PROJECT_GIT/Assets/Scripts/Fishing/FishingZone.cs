
using System;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private PlayerMovement _player;

    [SerializeField] private FishingSystem _fishingSystem;
    [SerializeField] private GameObject _interactButton;
    

    public event Action OnPlayerInZone;


    private void Start()
    {
        _player = FindFirstObjectByType<PlayerMovement>();
        _sprite = transform.GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // _interactButton.SetActive(true);
        if (other.gameObject.CompareTag("Player"))
        {
            _sprite.color = Color.green;
            OnPlayerInZone?.Invoke();

            // Проверяем, есть ли выбранная наживка
            Item selectedItem = SelectedItemManager.Instance.selectedItem;

            if (selectedItem != null && selectedItem is BaitItem baitItem)
                //&& _player.test goes inside if statement(check for minigame input)
            {
                // Проверяем, есть ли такая наживка в инвентаре
                bool hasBait = Inventory.instance.HasItem(baitItem, baitItem.itemType);

                if (hasBait)
                {
                    // Запускаем мини-игру
                    _fishingSystem.StartMiniGame();
                }
                else
                {
                    Debug.Log("Нет наживки для рыбалки.");
                    GameManager.ShowNoBaitMessage(); // Можно вызвать UI-оповещение
                }
            }
            else
            {
                Debug.Log("Выберите наживку перед рыбалкой.");
                GameManager.ShowNoBaitMessage(); // Можно вызвать UI-оповещение
            }

            // Здесь можно отключить движение игрока, если нужно
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _sprite.color = Color.yellow;
            _interactButton.SetActive(false);
        }
    }
}