using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public interface IInputService
{
    Vector2 MoveDirection { get; }
}