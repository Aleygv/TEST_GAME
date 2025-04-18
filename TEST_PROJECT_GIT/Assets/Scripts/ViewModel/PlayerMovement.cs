using UnityEngine;

public class PlayerMovement
{
    private PlayerAnimator _playerAnimator;
    private IInputService _inputService;

    public void Init(IInputService inputService, PlayerAnimator playerAnimator)
    {
        _playerAnimator = playerAnimator;
        _inputService = inputService;
    }
}
