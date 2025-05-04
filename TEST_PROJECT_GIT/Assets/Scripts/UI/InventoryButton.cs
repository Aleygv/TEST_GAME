using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Image inventoryWindow;

    private Button _button;
    private Image _buttonImage;
    
    private void OnEnable()
    {
        _buttonImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        GameManager.OnStartFishing += HideButton;
        GameManager.OnStopFishing += ShowButton;
    }

    private void OnDisable()
    {
        GameManager.OnStartFishing -= HideButton;
        GameManager.OnStopFishing -= ShowButton;
    }
    
    private void Awake()
    {
        HideWindow();
    }

    public void ShowWindow()
    {
        inventoryWindow.gameObject.SetActive(true);
        HideButton();
    }

    public void HideWindow()
    {
        inventoryWindow.gameObject.SetActive(false);
    }
    
    public void HideButton()
    {
        Color c = _buttonImage.color;
        c.a = 0f;
        _buttonImage.color = c;
        _button.interactable = false;
    }
    
    public void ShowButton()
    {
        Color c = _buttonImage.color;
        c.a = 1f;
        _buttonImage.color = c;
        _button.interactable = true;
    }
}
