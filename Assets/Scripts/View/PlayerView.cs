using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private PlayerMovement _viewModel;
    [SerializeField] private TextMeshPro _currency;
    [SerializeField] private TextMeshPro _fishCatchedValue;

    public void Initialize(PlayerData playerData)
    {
        playerData.ChangedCurrencyValue += PlayerData_ChangedCurrencyValue;
        playerData.ChangedFishCatchedValue += PlayerData_ChangedFishCatchedValue;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //transform.position = _viewModel.Position;

        // ????????????? ????? ??????:
        //_rb.linearVelocity = _viewModel.Velocity;
    }

    private void PlayerData_ChangedFishCatchedValue(int obj)
    {
            
    }

    private void PlayerData_ChangedCurrencyValue(int obj)
    {
            
    }

    public void Initialize(PlayerMovement viewModel)
    {
        _viewModel = viewModel;
    }
}
