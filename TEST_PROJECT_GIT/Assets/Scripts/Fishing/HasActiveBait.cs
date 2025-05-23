using System.Collections;
using UnityEngine;

public class HasActiveBait : MonoBehaviour
{
    [SerializeField] private GameObject hasNoBait; // UI объект, который будет появляться

    private const float TIME_TO_SHOW_WINDOW = 2f;

    private void Start()
    {
        if (hasNoBait != null)
            hasNoBait.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnHasBait += ShowMessage;
    }

    private void OnDisable()
    {
        GameManager.OnHasBait -= ShowMessage;
    }

    public void ShowMessage()
    {
        // Запускаем корутину отображения сообщения
        StartCoroutine(ShowMessageCoroutine());
    }

    private IEnumerator ShowMessageCoroutine()
    {
        if (hasNoBait != null)
        {
            hasNoBait.SetActive(true);

            yield return new WaitForSeconds(TIME_TO_SHOW_WINDOW);

            hasNoBait.SetActive(false);
        }
    }
}