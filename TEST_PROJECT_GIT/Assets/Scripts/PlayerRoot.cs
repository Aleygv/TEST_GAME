using System;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    [SerializeField] private PlayerView _playerPrefab;

    private IInputService _inputService;
    private PlayerData _model;
    private PlayerMovement _viewModel;

    private void Start()
    {
        // Создаем все компоненты
        // 1. Создаем сервис
        var inputService = new InputService();

        // 2. Создаем модель
        var model = new PlayerData();

        // 3. Связываем через ViewModel (здесь ошибка была бы явной)
        var viewModel = new PlayerMovement();

        // 4. Инициализируем View
        var playerView = Instantiate(_playerPrefab);
        playerView.Initialize(viewModel);
    }

    
}