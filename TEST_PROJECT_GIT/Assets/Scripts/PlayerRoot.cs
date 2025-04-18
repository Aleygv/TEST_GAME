using UnityEngine;

public class PlayerRoot
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerData _playerData;

    private void Init(IInputService inputService)
    {
        _playerMovement.Init(inputService, _playerAnimator);
        _playerView.Init(_playerData);
    }
}
