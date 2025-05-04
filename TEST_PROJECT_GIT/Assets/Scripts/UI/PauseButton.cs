using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseWindow;

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
        pauseWindow.SetActive(true);
    }

    public void HideWindow()
    {
        pauseWindow.SetActive(false);
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
