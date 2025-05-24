using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;


public class detect_swipe : MonoBehaviour
{
    public static detect_swipe instance;
    public delegate void Swipe(Vector2 direction);
    public event Swipe swipePerformed;

    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeResistance = 50;

    private Vector2 currentPosition;
    private Vector2 initialPosition;

    
    private void Awake()
    {
        position.Enable();
        press.Enable();
        press.performed += _ => { initialPosition = currentPosition; };
        press.canceled += _ => DetectSwipe();
        instance = this;
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPosition - initialPosition;
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(delta.x) < swipeResistance)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
        }
        if(Mathf.Abs(delta.y) < swipeResistance)
        {
            direction.y = Mathf.Clamp(delta.y, -1, 1);
        }
        if (direction != Vector2.zero && swipePerformed != null)
        {
            swipePerformed(direction);
        }

    }


 
}
