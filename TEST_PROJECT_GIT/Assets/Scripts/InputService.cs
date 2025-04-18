using System;
using UnityEngine;

public class InputService : IInputService
{
    public event Action Interact;
    public Vector2 MoveDirection { get; set; }
    
}
