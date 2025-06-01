using System.Collections;
using UnityEngine;

public class CheckFishToSell : MonoBehaviour
{
    [SerializeField] private GameObject fishToSellWindow;

    private const float TIME_TO_SHOW_WINDOW = 2f;

    private void Start()
    {
        if (fishToSellWindow != null)
            fishToSellWindow.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnHasFishToSell += ShowMessage;
    }

    private void OnDisable()
    {
        GameManager.OnHasFishToSell -= ShowMessage;
    }

    public void ShowMessage()
    {
        // Запускаем корутину отображения сообщения
        StartCoroutine(ShowMessageCoroutine());
    }

    private IEnumerator ShowMessageCoroutine()
    {
        if (fishToSellWindow != null)
        {
            fishToSellWindow.SetActive(true);

            yield return new WaitForSeconds(TIME_TO_SHOW_WINDOW);

            fishToSellWindow.SetActive(false);
        }
    }
}