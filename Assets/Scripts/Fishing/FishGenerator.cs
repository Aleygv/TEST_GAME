using System;
using Random = UnityEngine.Random;

public static class FishGenerator
{
    public static event Action OnFishCaught;

    public static void CaughtFish()
    {
        OnFishCaught?.Invoke();
    }
    
    public static int GenerateFish(int levelOfBait)
    {
        if (levelOfBait == 1)
        {
            return 1;
        }

        if (levelOfBait >= 2)
        {
            return Random.value < 0.4f ? levelOfBait - 1 : levelOfBait; //40% на рыбу уровня меньше
        }

        return 0;
    }
}