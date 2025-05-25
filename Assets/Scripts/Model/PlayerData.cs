using System;   
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private int _currencyValue = 0;
    private int _fishCatchedValue = 0;
    public event Action<int> ChangedCurrencyValue; 
    public event Action<int> ChangedFishCatchedValue;

    //Для зависимостей
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    public float Speed { get; } = 5f;

    public void Move(Vector2 direction)
    {
        Velocity = direction * Speed;
        Position += Velocity * Time.deltaTime;
    }


}
