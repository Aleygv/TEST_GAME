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

    [Header("Note Items")] [SerializeField]
    private NoteItem[] noteItems; // Перетянуть 5 NoteItem из проекта

    [Header("User Data (for demo)")] [SerializeField]
    private string currentLogin = "test15"; // Текущий логин

    [SerializeField] private string authToken = ""; // Берётся из PlayerPrefs

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

        var request = new GenerateTextRequest
        {
            auth_token = authToken,
            login = currentLogin,
            prompt = "story about the rock",
            max_tokens = 600
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
                Debug.LogError($"Ошибка получения записок: {webRequest.error}");
            }
            else
            {
                GenerateTextResponse response =
                    JsonUtility.FromJson<GenerateTextResponse>(webRequest.downloadHandler.text);
                SplitAndAssignNotes(response.text);
            }
        }
    }

    private void SplitAndAssignNotes(string fullText)
    {
        string[] parts = SplitText(fullText, noteItems.Length);

        for (int i = 0; i < noteItems.Length && i < parts.Length; i++)
        {
            noteItems[i].content = parts[i];
            Debug.Log($"Note {i}: {parts[i]}");
        }
    }

    private string[] SplitText(string text, int partsCount)
    {
        string[] result = new string[partsCount];
        int length = text.Length / partsCount;
        for (int i = 0; i < partsCount; i++)
        {
            int start = i * length;
            int end = (i == partsCount - 1) ? text.Length : start + length;

            // Попробуем найти ближайший пробел после end
            int safeEnd = text.IndexOf(' ', end + 10);
            if (safeEnd == -1 || safeEnd > text.Length) safeEnd = text.Length;

            result[i] = text.Substring(start, safeEnd - start).Trim();
        }

        return result;
    }
}