using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ArrowBTN", menuName = "FishUI")]
public class ArrowButton : ScriptableObject
{
    [SerializeField] private Image _arrowImage;
    [SerializeField] private ArrowDirection _direction;

    public enum ArrowDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public Image ArrowImage
    {
        get => _arrowImage;
        set => _arrowImage = value;
    }

    public Vector2 Direction
    {
        get
        {
            switch (_direction)
            {
                case ArrowDirection.Up:
                    return Vector2.up;
                case ArrowDirection.Down:
                    return Vector2.down;
                case ArrowDirection.Left:
                    return Vector2.left;
                case ArrowDirection.Right:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }
    }
}
