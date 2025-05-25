using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingProgress : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;

    public static FishingProgress Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddToProgressBar(float progress)
    {
        progressBarImage.fillAmount += progress;
    }

    public void ResetProgress()
    {
        progressBarImage.fillAmount = 0;
    }

    public float GetProgress()
    {
        return progressBarImage.fillAmount;
    }

    public bool IsFillOver()
    {
        return GetProgress() >= 1;
    }

    public bool IsNotFilled()
    {
        return GetProgress() <= 0;
    }

}