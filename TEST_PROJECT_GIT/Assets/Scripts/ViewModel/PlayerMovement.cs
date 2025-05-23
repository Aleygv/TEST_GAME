using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;


    private Animator _animator;
    private ContactFilter2D _contactFilter;
    private Rigidbody2D _rb;
    private IInputService _inputService;


    private PlayerInput _playerInput;
    private Input_presystem inputSys;


    private FishingSystem _fishingSystem;



    private void Awake()
    {
        //MESS DO NOT TOUCH(or do, i'm not a CEO)
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;


        _playerInput = GetComponent<PlayerInput>();
        inputSys = new Input_presystem();
        inputSys.Player.Enable();
        inputSys.Player.interact.performed += interact;
        // inputSys.Player.Move.performed += MovePerformed;


        //_inputService = new InputService(); // Creating an input implementation

    }

    public void Init(Animator animator)
    {
        _animator = animator;

    }

    private void FixedUpdate()
    {
        //Move();

        Vector2 inputVector = inputSys.Player.Move.ReadValue<Vector2>();

        _rb.linearVelocity = new Vector2(inputVector.x * _moveSpeed, inputVector.y * _moveSpeed);
        if (_rb.linearVelocity != Vector2.zero)
        {
            _animator.SetFloat("MoveX", _rb.linearVelocity.x);
            _animator.SetFloat("MoveY", _rb.linearVelocity.y);
        }

    }





    public void MovePerformed(InputAction.CallbackContext call)
    {
        Debug.Log(call);

    }


    public void interact(InputAction.CallbackContext call)
    {
        Debug.Log("DO SOMETHING!!!!!!!  " + call.phase);
        //_fishingSystem.StartMiniGame();
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