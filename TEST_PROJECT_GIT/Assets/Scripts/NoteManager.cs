using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GenerateTextResponse
{
    public string text;
}

[Serializable]
public class GenerateTextRequest
{
    public string auth_token;
    public string login;
    public string prompt;
    public int max_tokens;
}

public class NoteManager : MonoBehaviour
{
    private const string BaseUrl = "http://5.35.89.153";

    [Header("Note Items")]
    [SerializeField] private NoteItem[] noteItems; // Перетянуть 5 NoteItem из проекта

    [Header("User Data (for demo)")]
    [SerializeField] private string currentLogin = "test15"; // Текущий логин
    [SerializeField] private string authToken = ""; // Берётся из PlayerPrefs

    [Header("Prompts for Notes")]
    [SerializeField] private string[] prompts; // Например: ["story about the rock", "history of the forest", ...]

    private static NoteManager _instance;
    public static NoteManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            authToken = PlayerPrefs.GetString("auth_token", "");
            StartCoroutine(LoadNotes());
        }
    }

    public void ReloadNotes(string newAuthToken, string newLogin)
    {
        authToken = newAuthToken;
        currentLogin = newLogin;
        PlayerPrefs.SetString("auth_token", authToken);
        StartCoroutine(LoadNotes());
    }

    private IEnumerator LoadNotes()
    {
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("Нет токена для получения записок.");
            yield break;
        }

        if (noteItems == null || noteItems.Length == 0)
        {
            Debug.LogWarning("Нет записок для загрузки.");
            yield break;
        }

        if (prompts == null || prompts.Length < noteItems.Length)
        {
            Debug.LogWarning("Мало промптов для всех записок. Будут использованы дефолтные.");
        }

        for (int i = 0; i < noteItems.Length; i++)
        {
            string prompt = i < prompts.Length ? prompts[i] : $"story about item {i + 1}";

            yield return SendGenerateRequestForNote(i, prompt);
        }

        Debug.Log("Все записки загружены!");
    }

    private IEnumerator SendGenerateRequestForNote(int noteIndex, string prompt)
    {
        var request = new GenerateTextRequest
        {
            auth_token = authToken,
            login = currentLogin,
            prompt = prompt,
            max_tokens = 1600
        };

        string jsonBody = JsonUtility.ToJson(request);

        using (UnityWebRequest webRequest = new UnityWebRequest(BaseUrl + "/qwen/generate", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка при генерации записки {noteIndex}: {webRequest.error}");
                noteItems[noteIndex].content = "Не удалось загрузить текст.";
            }
            else
            {
                GenerateTextResponse response =
                    JsonUtility.FromJson<GenerateTextResponse>(webRequest.downloadHandler.text);
                noteItems[noteIndex].content = response.text;
                Debug.Log($"Записка {noteIndex} загружена: {response.text.Substring(0, Mathf.Min(50, response.text.Length))}...");
            }
        }
    }
}