using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshPro _currency;
    [SerializeField] private TextMeshPro _fishCatchedValue;

    public void Init(PlayerData playerData)
    {
        playerData.ChangedCurrencyValue += PlayerData_ChangedCurrencyValue;
        playerData.ChangedFishCatchedValue += PlayerData_ChangedFishCatchedValue;
    }

    private void PlayerData_ChangedFishCatchedValue(int obj)
    {
            
    }

    private void PlayerData_ChangedCurrencyValue(int obj)
    {
            
    }
}
