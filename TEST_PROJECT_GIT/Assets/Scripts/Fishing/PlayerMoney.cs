using System;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private int _currencyValue = 0;
    public int Value
    {
        get => _currencyValue;
        set 
        {
            if (_currencyValue != value)
            {
                _currencyValue = value;
                Debug.Log($"Currency value changed: {_currencyValue}");
            }
        }
    }
    
    public static PlayerMoney Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    
    public void AddMoney(int amount)
    {
        _currencyValue += amount;
        Debug.Log($"Added {amount} money. Total: {_currencyValue}");
    }
}
