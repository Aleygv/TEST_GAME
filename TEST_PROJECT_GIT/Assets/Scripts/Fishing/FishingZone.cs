using System;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private PlayerMovement _player;

    [SerializeField] private FishingSystem _fishingSystem;

    public event Action OnPlayerInZone;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerMovement>();
        _sprite = transform.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _sprite.color = Color.green;
            OnPlayerInZone?.Invoke();
            _fishingSystem.StartMiniGame();
            // Here you can disable Player Movement
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _sprite.color = Color.yellow;
            
        }
    }
}
