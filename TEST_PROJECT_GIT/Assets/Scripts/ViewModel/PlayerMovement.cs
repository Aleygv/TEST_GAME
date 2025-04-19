using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;




[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]

public class FloatingJoystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform RectTransform;
    public RectTransform Knob;


    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
//result of merged classes(FloatingJoystick and PlayerMovement)


public class PlayerMovement : MonoBehaviour
{
    private const string TYPE_OF_TAG = "Wall";

    [SerializeField]
    private FloatingJoystick Joystick;
    [SerializeField]
    private Vector2 joystickSize = new Vector2(12, 12);

    private Finger MovementFinger;
    private Vector2 MovementAmount;
    private Rigidbody2D _rigidbody2D;
    private float moveSpeed = 10f;



    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += Touch_onFingerDown;
        ETouch.Touch.onFingerUp += Touch_onFingerUp;
        ETouch.Touch.onFingerMove += Move;
    }

    public void OnDisable()
    {
        ETouch.Touch.onFingerDown -= Touch_onFingerDown;
        ETouch.Touch.onFingerUp -= Touch_onFingerUp;
        ETouch.Touch.onFingerMove -= Move;
        EnhancedTouchSupport.Disable();
    }







    private void Move(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                currentTouch.screenPosition,
                Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                    ).normalized
                * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }

    }

    private void Touch_onFingerUp(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void Touch_onFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = joystickSize;
            Joystick.RectTransform.anchoredPosition = (Vector2)ClampStartPosition(TouchedFinger.screenPosition);
//          Joystick.RectTransform.anchoredPosition = (TouchedFinger.screenPosition - TouchedFinger.screenPosition.x - TouchedFinger.screenPosition.y); 
        }
    }

    private object ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < joystickSize.x / 2)
        {
            StartPosition.x = joystickSize.x / 2;
        }

        if (StartPosition.y < joystickSize.y / 2)
        {
            StartPosition.y = joystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - joystickSize.y / 2)
        {
            StartPosition.y = Screen.height - joystickSize.y / 2;
        }

        return StartPosition;
    }

    private void Update()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + MovementAmount * (Time.fixedDeltaTime * moveSpeed));
    }

}
