using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 500f;
    
    
    private float swipeResistance = 5;
    
    
    private Animator _animator;
    private ContactFilter2D _contactFilter;
    private Rigidbody2D _rb;
    private IInputService _inputService;
    private Input_presystem inputSys;
    private bool isWalking;
    private bool isFishing;
    private FishingZone _fishingZone;

    public bool test;
    

    private void Awake()
    {
        //MESS DO NOT TOUCH(or do, i'm not a CEO)
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Setup BoxCollider2D
        var collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.size = new Vector2(0.8f, 0.8f); // Adjust size to match your sprite

        // Setup Input System
        inputSys = new Input_presystem();
        inputSys.Player.Enable();
        inputSys.Player.interact.performed += interact;
        inputSys.Player.Move.performed += MovePerformed;
        inputSys.Player.Move.canceled += MoveCanceled;
        inputSys.Minigame.swipe.performed += SwipePerformed;

        gameObject.layer = LayerMask.NameToLayer("Player");
        
        _fishingZone = FindFirstObjectByType<FishingZone>();
        test = false;

        // Подписываемся на события окончания рыбалки
        GameManager.OnGameWin += OnFishingEnd;
        GameManager.OnGameLose += OnFishingEnd;
    }

    private void OnDestroy()
    {
        if (inputSys != null)
        {
            inputSys.Player.interact.performed -= interact;
            inputSys.Player.Move.performed -= MovePerformed;
            inputSys.Player.Move.canceled -= MoveCanceled;
            inputSys.Minigame.swipe.performed -= SwipePerformed;
            inputSys.Dispose();
        }

        // Отписываемся от событий
        GameManager.OnGameWin -= OnFishingEnd;
        GameManager.OnGameLose -= OnFishingEnd;
    }

    private void OnFishingEnd()
    {
        test = false;
        isFishing = false;
        inputSys.Minigame.Disable();
    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }

   



    private void FixedUpdate()
    {
        Vector2 inputVector = inputSys.Player.Move.ReadValue<Vector2>();
        
        // Update movement velocity
        _rb.linearVelocity = new Vector2(inputVector.x * _moveSpeed * Time.deltaTime, inputVector.y * _moveSpeed * Time.deltaTime);
        
        // Update animator parameters
        if (_rb.linearVelocity != Vector2.zero)
        {
            _animator.SetFloat("MoveX", inputVector.x);
            _animator.SetFloat("MoveY", inputVector.y);
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
    }

    public void interact(InputAction.CallbackContext call)
    {
        bool smth = _fishingZone != null;
        Debug.Log(smth  + "   "  + _fishingZone.IsPlayerInZone);
        // Проверяем, находимся ли мы в зоне рыбалки и можем ли начать рыбалку
        if (_fishingZone != null && _fishingZone.IsPlayerInZone)
        {
            if (!test && _fishingZone.CanStartFishing())
            {
                test = true;
                isFishing = true;
                Debug.Log("MINIGAME!!!");
                inputSys.Minigame.Enable();
                _fishingZone.FishingSystem.StartMiniGame();
            }
            else if (test)
            {
                Debug.Log("DOH");
                inputSys.Minigame.Disable();
                test = false;
                isFishing = false;
            }
        }
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision enter if needed
        Debug.Log($"Collision with: {collision.gameObject.name}");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Handle ongoing collision if needed
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Handle collision exit if needed
    }


    public void SwipePerformed(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        if (test) 
        {
            if (vec.x > 0 && Math.Abs(vec.x) > Math.Abs(vec.y) && Math.Abs(vec.x) > swipeResistance)
            {
                Debug.Log("Right");
            }
            if (vec.x < 0 && Math.Abs(vec.x) > Math.Abs(vec.y) && Math.Abs(vec.x) > swipeResistance)
            {
                Debug.Log("Left");
            }
            if (vec.y > 0 && Math.Abs(vec.x) < Math.Abs(vec.y) && Math.Abs(vec.y) > swipeResistance)
            {
                Debug.Log("Up");
            }
            if (vec.y < 0 && Math.Abs(vec.x) < Math.Abs(vec.y) && Math.Abs(vec.y) > swipeResistance)
            {
                Debug.Log("Down");
            }
        }
    }
}



//    private bool isMoving;
//    private Rigidbody2D _rigidbody2D;

//    private void Awake()
//{
//    _rigidbody2D = GetComponent<Rigidbody2D>();
//}

//private void Start()
//{
//    isMoving = true;
//}

//private void FixedUpdate()
//{
//    Move();
//}


// 
//private void Move()
//{
//    if (isMoving)
//    {
//        float moveX = Input.GetAxisRaw("Horizontal");
//        float moveY = Input.GetAxisRaw("Vertical");

//        movement = new Vector2(moveX, moveY).normalized;

//        _rigidbody2D.MovePosition(_rigidbody2D.position + movement * (Time.fixedDeltaTime * moveSpeed));
//    }
//}

//private void OnCollisionEnter2D(Collision2D other)
//{
//    Debug.Log(other.gameObject.tag);
//}
//}