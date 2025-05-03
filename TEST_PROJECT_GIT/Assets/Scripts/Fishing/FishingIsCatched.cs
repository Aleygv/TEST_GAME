using System;
using TMPro;
using UnityEngine;

public class FishingIsCatched : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;

    public static FishingIsCatched Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowWin()
    {
        winText.gameObject.SetActive(true);
    }

    public void HideWin()
    {
        winText.gameObject.SetActive(false);
    }
    
    public void ShowLose()
    {
        loseText.gameObject.SetActive(true);
    }

    public void HideLose()
    {
        loseText.gameObject.SetActive(false);
    }
}
