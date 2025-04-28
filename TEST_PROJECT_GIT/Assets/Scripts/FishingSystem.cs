using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingSystem : MonoBehaviour
{
    public static event Action OnGameStartRequest;
    
    // Ссылки на UI-элементы
    [SerializeField] private GameObject fishingGameUI; // Панель с мини-игрой
    [SerializeField] private Image[] arrows; // Массив изображений стрелочек

    // Цвета для состояний стрелочек
    public Color grayColor = Color.gray;
    public Color whiteColor = Color.white;
    public Color redColor = Color.red;

    // Время на выполнение мини-игры
    public float timeLimit = 10f;

    // Текущая последовательность стрелочек
    private int currentArrowIndex = 0;

    // Таймер
    private float timer;

    // Флаг активности мини-игры
    private bool isPlaying = false;

    void Start()
    {
        // Скрываем панель мини-игры при старте
        fishingGameUI.SetActive(false);

        // Устанавливаем начальный цвет стрелочек
        foreach (var arrow in arrows)
        {
            arrow.color = grayColor;
        }

        OnGameStartRequest += StartMiniGame;
    }

    // Вызывается, когда игрок входит в зону мини-игры
    public void StartMiniGame()
    {
        if (!isPlaying)
        {
            // Активируем UI и сбрасываем состояние
            fishingGameUI.SetActive(true);
            ResetGame();

            // Запускаем таймер
            StartCoroutine(GameTimer());
            isPlaying = true;
        }
    }

    // Сброс игры
    private void ResetGame()
    {
        currentArrowIndex = 0;
        timer = timeLimit;

        // Сбрасываем цвета всех стрелочек
        foreach (var arrow in arrows)
        {
            arrow.color = grayColor;
        }
    }

    // Обработка ввода
    void Update()
    {
        if (isPlaying)
        {
            // Проверяем нажатия клавиш
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                CheckArrow(KeyCode.UpArrow);
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                CheckArrow(KeyCode.DownArrow);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                CheckArrow(KeyCode.LeftArrow);
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                CheckArrow(KeyCode.RightArrow);
        }
    }

    // Проверка нажатия на стрелочку
    private void CheckArrow(KeyCode key)
    {
        // Определяем ожидаемую стрелочку
        KeyCode expectedKey = GetExpectedKey(currentArrowIndex);

        if (key == expectedKey)
        {
            // Правильное нажатие
            arrows[currentArrowIndex].color = whiteColor;
            currentArrowIndex++;

            // Проверяем, завершена ли мини-игра
            if (currentArrowIndex >= arrows.Length)
            {
                EndGame(true); // Успех
            }
        }
        else
        {
            // Неправильное нажатие
            foreach (var arrow in arrows)
            {
                arrow.color = redColor;
            }
            currentArrowIndex = 0; // Сбрасываем прогресс
        }
    }

    // Получение ожидаемой клавиши для текущей стрелочки
    private KeyCode GetExpectedKey(int index)
    {
        switch (index)
        {
            case 0: return KeyCode.UpArrow;    // ↑
            case 1: return KeyCode.DownArrow;  // ↓
            case 2: return KeyCode.LeftArrow;  // ←
            case 3: return KeyCode.RightArrow; // →
            default: return KeyCode.None;
        }
    }

    // Таймер мини-игры
    private IEnumerator GameTimer()
    {
        while (timer > 0 && isPlaying)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Если время истекло, завершаем игру неудачей
        if (isPlaying)
        {
            EndGame(false);
        }
    }

    // Завершение мини-игры
    private void EndGame(bool success)
    {
        isPlaying = false;
        fishingGameUI.SetActive(false);

        if (success)
        {
            Debug.Log("Мини-игра пройдена успешно!");
            // Добавьте логику для успешного завершения (например, награда)
        }
        else
        {
            Debug.Log("Мини-игра провалена.");
            // Добавьте логику для провала
        }
    }
}