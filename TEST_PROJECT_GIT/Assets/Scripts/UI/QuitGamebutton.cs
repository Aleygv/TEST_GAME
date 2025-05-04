using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private void Start()
    {
        // Получаем компонент Button и подписываемся на событие нажатия
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("Компонент Button не найден на объекте: " + gameObject.name);
        }
    }

    private void QuitGame()
    {
        Debug.Log("Выход из игры...");

#if UNITY_EDITOR
        // Останавливает воспроизведение в редакторе Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Выход из приложения (работает в билде)
        Application.Quit();
#endif
    }
}