using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderButton : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void LoadScene()
    {
        Debug.Log($"Попытка загрузить сцену по индексу: {sceneIndex}");

        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
            Debug.Log($"Сцена загружена успешно!");
        }
        else
        {
            Debug.LogError($"Неверный индекс сцены: {sceneIndex}");
        }
    }
}