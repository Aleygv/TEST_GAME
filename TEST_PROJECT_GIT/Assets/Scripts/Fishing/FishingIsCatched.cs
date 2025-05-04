using System;
using System.Collections;
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

    private void Start()
    {
        HideWin();
        HideLose();

        GameManager.OnGameWin += ShowWin;
        GameManager.OnGameLose += ShowLose;
    }

    public void ShowWin()
    {
        winText.gameObject.SetActive(true);
        StartCoroutine(ShowWinWindow());
    }

    public void HideWin()
    {
        winText.gameObject.SetActive(false);
    }
    
    public void ShowLose()
    {
        loseText.gameObject.SetActive(true);
        StartCoroutine(ShowWLoseWindow());
    }

    public void HideLose()
    {
        loseText.gameObject.SetActive(false);
    }

    private IEnumerator ShowWinWindow()
    {
        yield return new WaitForSeconds(2f);
        HideWin();
    }
    
    private IEnumerator ShowWLoseWindow()
    {
        yield return new WaitForSeconds(2f);
        HideLose();
    }
}
