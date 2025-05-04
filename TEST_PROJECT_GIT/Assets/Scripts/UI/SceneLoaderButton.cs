using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        Debug.Log($"Попытка загрузить сцену: {sceneName}");

        // Диагностика: вывод всех сцен в билде
        Debug.Log($"Доступные сцены в билде:");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            Debug.Log($"{i}: {name}");
        }

        if (SceneManager.GetSceneByName(sceneName).IsValid())
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Сцена с именем '{sceneName}' не найдена в Build Settings!");
        }
    }
}