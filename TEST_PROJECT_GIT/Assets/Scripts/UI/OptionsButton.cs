using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    
    private void Awake()
    {
        HideWindow();
    }

    public void ShowWindow()
    {
        optionsWindow.gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        optionsWindow.gameObject.SetActive(false);
    }
}
