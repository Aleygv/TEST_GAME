using System;
using Random = UnityEngine.Random;

public static class FishGenerator
{
    public static event Action OnFishCaught;

    public static void CaughtFish()
    {
        OnFishCaught?.Invoke();
    }

    //Эвент для устанавления наживки на удочки в данный момент
    public static event Action OnBaitChanged;

    public static void BaitChange()
    {
        OnBaitChanged?.Invoke();
    }
    
    public static int GenerateFish(int levelOfBait)
    {
        if (levelOfBait == 1)
        {
            return 1;
        }

        if (levelOfBait >= 2)
        {
            return Random.value < 0.5f ? levelOfBait - 1 : levelOfBait;
        }

        return 0;
    }
}