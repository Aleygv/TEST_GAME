using System;
using UnityEngine;

public interface IInputService
{
    event Action Interact;
    Vector2 MoveDirection { get; set; }
}
