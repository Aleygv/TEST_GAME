using UnityEngine;

public class BaitSlot : MonoBehaviour
{
    private BaitItem currentBait;

    public void SetBait(BaitItem bait)
    {
        currentBait = bait;
    }

    public BaitItem GetBait()
    {
        return currentBait;
    }
}