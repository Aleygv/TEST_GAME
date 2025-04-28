using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private IInputService _inputService;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _inputService = new InputService(); // Creating an input implementation

    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    private void Update()
    {
        Move();
        
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(
            _inputService.MoveDirection.x * _moveSpeed,
            _inputService.MoveDirection.y * _moveSpeed
        );
        if(_rb.linearVelocity != Vector2.zero )
        {
            _animator.SetFloat("MoveX", _rb.linearVelocity.x);
            _animator.SetFloat("MoveY", _rb.linearVelocity.y);
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