using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    
    private float swipeResistance = 5;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private Input_presystem inputSys;
    private bool isWalking;
    private bool isFishing;
    private FishingZone _fishingZone;
    private Vector2 moveDirection;

    public bool test;

    private void Awake()
    {
        // Setup Rigidbody2D for proper collisions
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _rb.collisionDetection = CollisionDetectionMode2D.Continuous;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Setup BoxCollider2D
        var collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.size = new Vector2(0.8f, 0.8f);
        
        // Set layer to avoid self-collision
        gameObject.layer = LayerMask.NameToLayer("Player");
        
        SetupInputSystem();
        
        _fishingZone = FindFirstObjectByType<FishingZone>();
        test = false;

        GameManager.OnGameWin += OnFishingEnd;
        GameManager.OnGameLose += OnFishingEnd;
    }

    private void SetupInputSystem()
    {
        if (inputSys != null)
        {
            inputSys.Dispose();
        }
        
        inputSys = new Input_presystem();
        inputSys.Player.Enable();
        inputSys.Player.interact.performed += interact;
        inputSys.Player.Move.performed += MovePerformed;
        inputSys.Player.Move.canceled += MoveCanceled;
        inputSys.Minigame.swipe.performed += SwipePerformed;
    }

    private void OnDestroy()
    {
        CleanupInputSystem();
        
        GameManager.OnGameWin -= OnFishingEnd;
        GameManager.OnGameLose -= OnFishingEnd;
    }

    private void OnDisable()
    {
        CleanupInputSystem();
    }

    private void CleanupInputSystem()
    {
        if (inputSys != null)
        {
            inputSys.Player.Disable();
            inputSys.Minigame.Disable();
            
            inputSys.Player.interact.performed -= interact;
            inputSys.Player.Move.performed -= MovePerformed;
            inputSys.Player.Move.canceled -= MoveCanceled;
            inputSys.Minigame.swipe.performed -= SwipePerformed;
            
            inputSys.Dispose();
            inputSys = null;
        }
    }

    private void OnFishingEnd()
    {
        test = false;
        isFishing = false;
        if (inputSys != null)
        {
            inputSys.Minigame.Disable();
        }
    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    private void FixedUpdate()
    {
        if (inputSys == null) return;
        
        Vector2 inputVector = inputSys.Player.Move.ReadValue<Vector2>();
        moveDirection = inputVector.normalized;
        
        // Move the player using MovePosition for more precise physics movement
        if (moveDirection != Vector2.zero)
        {
            Vector2 newPosition = _rb.position + moveDirection * _moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(newPosition);
            
            _animator.SetFloat("MoveX", moveDirection.x);
            _animator.SetFloat("MoveY", moveDirection.y);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        
        _animator.SetBool("IsWalking", isWalking);
        _animator.SetBool("IsFishing", isFishing);
    }

    public void MovePerformed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        isWalking = inputVector != Vector2.zero;
    }

    public void MoveCanceled(InputAction.CallbackContext context)
    {
        isWalking = false;
        moveDirection = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with: {collision.gameObject.name} (Layer: {collision.gameObject.layer})");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Handle ongoing collision if needed
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Handle collision exit if needed
    }

    // ... rest of your existing code for fishing and swipe handling ...
} 