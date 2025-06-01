using System;

public static class GameManager
{
    public static event Action OnGameWin;
    public static event Action OnGameLose;

    public static void FishingWin()
    {
        OnGameWin?.Invoke();
    }

    public static void FishingLose()
    {
        OnGameLose?.Invoke();
    }

    public static event Action OnStartFishing;
    public static event Action OnStopFishing;

    public static void StartFishing()
    {
        OnStartFishing?.Invoke();
    }

    public static void StopFishing()
    {
        OnStopFishing?.Invoke();
    }

    public static event Action OnHasBait;

    public static void ShowNoBaitMessage()
    {
        OnHasBait?.Invoke();
    }

    public static event Action OnHasFishToSell;
    
    public static void ShowHasFishToSellMessage()
    {
        OnHasFishToSell?.Invoke();
    }
}