using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }
    
    public void HideWindow()
    {
        gameObject.SetActive(false);
    }
    
    public void UpdateMoney(int money)
    {
        moneyText.text = money + "$";
    }
}
