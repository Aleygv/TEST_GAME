using UnityEngine;

[CreateAssetMenu(fileName = "Bait", menuName = "Item/Bait")]
public class BaitItem : Item
{
    public int levelOfBait;

    public override void Use()
    {
        Debug.Log("Используется новая наживка");
    }
}
