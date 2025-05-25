using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [Header("Main Menu")] [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    [Header("Registration Menu")] [SerializeField]
    private GameObject registrationElement;

    [SerializeField] private TMP_InputField dateBirthField;
    [SerializeField] private TMP_InputField loginFieldReg;
    [SerializeField] private TMP_InputField passwordFieldReg;

    [Header("Login Menu")] [SerializeField]
    private GameObject loginElement;

    [SerializeField] private TMP_InputField loginFieldLogin;
    [SerializeField] private TMP_InputField passwordFieldLogin;

    [Header("Error Texts")] [SerializeField]
    private TextMeshProUGUI errorTextReg;

    [SerializeField] private TextMeshProUGUI errorTextLogin;

    [Header("Auto Login Settings")] [SerializeField]
    private bool autoLoginOnStart = true;

    private const string BaseUrl = "http://5.35.89.153";

    private void Start()
    {
        registrationElement.SetActive(false);
        loginElement.SetActive(false);

        if (autoLoginOnStart)
        {
            string token = PlayerPrefs.GetString("auth_token", "");
            if (!string.IsNullOrEmpty(token))
            {
                Debug.Log("Токен найден. Автовход...");
                StartCoroutine(LoadMainMenu());
            }
        }

        loginButton.onClick.AddListener(OpenLoginMenu);
        registerButton.onClick.AddListener(OpenRegistrationMenu);
    }

    public void OpenRegistrationMenu()
    {
        mainMenu.SetActive(false);
        registrationElement.SetActive(true);
    }

    public void OpenLoginMenu()
    {
        mainMenu.SetActive(false);
        loginElement.SetActive(true);
    }

    public void OnRegisterButton()
    {
        ClearError(errorTextReg);

        string login = loginFieldReg.text.Trim();
        string password = passwordFieldReg.text.Trim();
        string birthDate = dateBirthField.text.Trim();

        if (string.IsNullOrEmpty(login))
        {
            ShowError("Логин не может быть пустым.", errorTextReg);
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowError("Пароль не может быть пустым.", errorTextReg);
            return;
        }

        if (string.IsNullOrEmpty(birthDate) || !DateTime.TryParse(birthDate, out _))
        {
            ShowError("Неверная дата рождения. Используйте формат YYYY-MM-DD.", errorTextReg);
            return;
        }

        StartCoroutine(RegisterCoroutine(login, password, birthDate));
    }

    public void OnLoginButton()
    {
        ClearError(errorTextLogin);

        string login = loginFieldLogin.text.Trim();
        string password = passwordFieldLogin.text.Trim();

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            ShowError("Логин и пароль обязательны.", errorTextLogin);
            return;
        }

        StartCoroutine(LoginCoroutine(login, password));
    }

    private IEnumerator RegisterCoroutine(string login, string password, string birthDate)
    {
        var url = BaseUrl + "/auth/register";
        var userData = new RegistrationData
        {
            login = login,
            password = password,
            birth_date = birthDate
        };
        string jsonBody = JsonUtility.ToJson(userData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                Debug.Log("Регистрация успешна.");
                ShowError("Регистрация успешна!", errorTextReg, Color.green);
                StartCoroutine(LoadMainMenu());
            }
            else if (request.responseCode == 400)
            {
                ShowError("Ошибка: пользователь уже существует или данные неверны.", errorTextReg);
            }
            else
            {
                ShowError($"Ошибка регистрации: {request.error}, код: {request.responseCode}", errorTextReg);
            }
        }
    }

    private IEnumerator LoginCoroutine(string login, string password)
    {
        var url = BaseUrl + "/auth/login";
        var userData = new LoginData
        {
            login = login,
            password = password
        };
        string jsonBody = JsonUtility.ToJson(userData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                Debug.Log("Вход успешен. Токен: " + authResponse.auth_token);

                PlayerPrefs.SetString("auth_token", authResponse.auth_token);
                PlayerPrefs.Save();

                ShowError("Вход выполнен успешно!", errorTextLogin, Color.green);
                StartCoroutine(LoadMainMenu());
            }
            else
            {
                ShowError($"Ошибка входа: {request.error}, код: {request.responseCode}", errorTextLogin);
            }
        }
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Переход в игру...");
        // SceneManager.LoadScene("MainGameScene");
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void Logout()
    {
        PlayerPrefs.DeleteKey("auth_token");
        PlayerPrefs.Save();

        mainMenu.SetActive(false);
        loginElement.SetActive(true);
        gameObject.SetActive(true);
    }

    private void ShowError(string message, TMP_Text errorText, Color? color = null)
    {
        if (errorText != null)
        {
            errorText.color = color ?? Color.red;
            errorText.text = message;
        }
        else
        {
            Debug.LogWarning(message);
        }
    }

    private void ClearError(TMP_Text errorText)
    {
        if (errorText != null)
        {
            errorText.text = "";
        }
    }
}

[Serializable]
public class RegistrationData
{
    public string login;
    public string password;
    public string birth_date;
}

[Serializable]
public class LoginData
{
    public string login;
    public string password;
}

[Serializable]
public class AuthResponse
{
    public string auth_token;
}