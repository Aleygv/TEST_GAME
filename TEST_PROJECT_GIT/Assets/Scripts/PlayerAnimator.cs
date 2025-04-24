using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMovement(Vector2 direction)
    {
        _animator.SetFloat("MoveX", direction.x);
        _animator.SetFloat("MoveY", direction.y);
    }
}