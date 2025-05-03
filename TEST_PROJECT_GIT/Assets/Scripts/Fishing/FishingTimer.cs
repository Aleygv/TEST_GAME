using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingTimer : MonoBehaviour
{
    public static FishingTimer Instance { get; set; }

    private readonly float _time = 8;
    [SerializeField] private float _timeToFish = 8;

    [SerializeField] private Image timerImage;
    
    private void Awake()
    {
        Instance = this;
    }
    
    // Таймер мини-игры
    public IEnumerator StartTimer()
    {
        while (_timeToFish > 0)
        {
            _timeToFish -= Time.deltaTime;
            timerImage.fillAmount = _timeToFish / _time;
            yield return null;
        }
    }

    public bool IsPenalty()
    {
        return _timeToFish <= 0;
    }

    public void ResetTimer()
    {
        _timeToFish = _time;
    }
}
