using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FishingTimer : MonoBehaviour
{
    public static FishingTimer Instance { get; set; }

    [SerializeField] private float timeToFish;
    private readonly float _time = 4;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void Awake()
    {
        Instance = this;
    }
    
    // Таймер мини-игры
    public IEnumerator StartTimer()
    {
        while (timeToFish > 0)
        {
            timeToFish -= Time.deltaTime;
            timerText.text = (timeToFish).ToString("F2");
            yield return null;
        }
    }

    public bool IsPenalty()
    {
        return timeToFish <= 0;
    }

    public void ResetTimer()
    {
        timeToFish = _time;
    }
}
