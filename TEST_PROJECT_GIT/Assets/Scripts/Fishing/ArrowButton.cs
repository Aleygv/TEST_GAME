using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ArrowBTN", menuName = "FishUI")]
public class ArrowButton : ScriptableObject
{
    [SerializeField] private KeyCode _arrowKey;
    [SerializeField] private Image _arrowImage;

    public Image ArrowImage
    {
        get => _arrowImage;
        set => _arrowImage = value;
    }

    public KeyCode ArrowKeyCode
    {
        get { return _arrowKey; }
        set { _arrowKey = value; }
    }
}
