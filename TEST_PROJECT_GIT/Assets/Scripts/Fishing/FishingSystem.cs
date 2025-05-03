using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingSystem : MonoBehaviour
{
    [SerializeField] private GameObject fishingGameUI; // Панель с мини-игрой
    [SerializeField] private Image[] arrowImages; // Массив изображений стрелочек

    [SerializeField] private ArrowButton[] arrowsPrefab;

    [SerializeField] private Color grayColor = Color.gray;
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private Color redColor = Color.red;

    [SerializeField] private float timeLimit = 10f;

    private int currentArrowIndex = 0;

    private float timer;

    private bool isPlaying = false;

    //Прогресс бар
    private float _progress = 0.0f;
    private readonly int _circlePrice = 20;
    private readonly int _circlePenalty = 15;
    private bool _isPerfect = true;
    private float _correctRatio;
    private float _currentCount = 0;
    private float _totalCount;
    private float _addedProgress;

    [SerializeField] private int repeatCount = 3; // Количество повторений
    private int currentRepeat = 0;


    void Start()
    {
        fishingGameUI.SetActive(false);
        FishingProgress.Instance.gameObject.SetActive(false);
        FishingTimer.Instance.gameObject.SetActive(false);
        // FishingIsCatched.Instance.HideWin();
        // FishingIsCatched.Instance.HideLose();
    }

    void Update()
    {
        if (isPlaying)
        {
            //Тут надо исправить ошибку с таймером, он не возобновляется по истечении времени
            if (IsTimeLeft())
            {
                FishingProgress.Instance.AddToProgressBar(_addedProgress);
            }

            // Проверяем все возможные клавиши
            foreach (var arrow in arrowsPrefab)
            {
                if (Input.GetKeyDown(arrow.ArrowKeyCode))
                {
                    CheckArrow(arrow.ArrowKeyCode);
                    break; // Прерываем цикл после обработки нажатия
                }
            }
        }
    }

    public void StartMiniGame()
    {
        if (!isPlaying)
        {
            fishingGameUI.SetActive(true);
            FishingProgress.Instance.gameObject.SetActive(true);
            FishingTimer.Instance.gameObject.SetActive(true);

            int randomArrowCount = Random.Range(4, 9);

            ArrowButton[] currentRoundArrows = GenerateRandomArrowSequence(randomArrowCount);
            _totalCount = currentRoundArrows.Length;

            AssignArrowData(currentRoundArrows);

            ResetGame();

            StartCoroutine(FishingTimer.Instance.StartTimer());

            isPlaying = true;

            currentRepeat = 0;
        }
    }

    private ArrowButton[] GenerateRandomArrowSequence(int count)
    {
        ArrowButton[] sequence = new ArrowButton[count];

        for (int i = 0; i < count; i++)
        {
            // Выбираем случайную стрелочку
            ArrowButton randomArrow = arrowsPrefab[Random.Range(0, arrowsPrefab.Length)];

            // Проверяем, чтобы не было более двух подряд одинаковых стрелочек
            if (i >= 2 && sequence[i - 1] == randomArrow && sequence[i - 2] == randomArrow)
            {
                // Если две предыдущие стрелочки такие же, выбираем другую
                while (randomArrow == sequence[i - 1])
                {
                    randomArrow = arrowsPrefab[Random.Range(0, arrowsPrefab.Length)];
                }
            }

            sequence[i] = randomArrow;
        }

        return sequence;
    }

    // Назначение данных стрелочкам
    private void AssignArrowData(ArrowButton[] sequence)
    {
        for (int i = 0; i < arrowImages.Length; i++)
        {
            if (i < sequence.Length)
            {
                // Назначаем спрайт и цвет
                arrowImages[i].sprite = sequence[i].ArrowImage.sprite;
                arrowImages[i].color = grayColor;
                arrowImages[i].gameObject.SetActive(true); // Активируем стрелочку
            }
            else
            {
                // Деактивируем лишние стрелочки
                arrowImages[i].gameObject.SetActive(false);
            }
        }
    }

    // Проверка нажатия на стрелочку
    private void CheckArrow(KeyCode key)
    {
        KeyCode expectedKey = GetExpectedKey(currentArrowIndex);

        float aimToFillProgressBar = Random.Range(4f, 8f);

        if (key == expectedKey)
        {
            arrowImages[currentArrowIndex].color = whiteColor;
            currentArrowIndex++;
            _currentCount++;

            // Проверяем, завершена ли текущая последовательность
            if (currentArrowIndex >= arrowImages.Length || !arrowImages[currentArrowIndex].gameObject.activeSelf)
            {
                FishingTimer.Instance.ResetTimer();
                _addedProgress = CalculateProgress(_correctRatio, _currentCount, _totalCount, _circlePenalty) /
                                 aimToFillProgressBar;
                FishingProgress.Instance.AddToProgressBar(_addedProgress);


                if (FishingProgress.Instance.IsNotFilled())
                {
                    EndGame(false); // Завершаем игру проигрышем
                    FishingProgress.Instance.ResetProgress();
                }

                // Увеличиваем счётчик повторений
                currentRepeat++;

                if (FishingProgress.Instance.IsFillOver())
                {
                    EndGame(true); // Завершаем игру успехом
                    FishingProgress.Instance.ResetProgress();
                }
                else
                {
                    ResetGame();
                    _isPerfect = true;
                }

                _currentCount = 0;
            }
        }


        else
        {
            // Неправильное нажатие: меняем все цвета на красные
            foreach (var arrow in arrowImages)
            {
                if (arrow.gameObject.activeSelf)
                {
                    arrow.color = redColor;
                }
            }

            _currentCount = 0;
            _isPerfect = false;

            // Сбрасываем прогресс
            StartCoroutine(ResetAfterError());
        }
    }

    // Получение ожидаемой клавиши для текущей стрелочки
    private KeyCode GetExpectedKey(int index)
    {
        Sprite currentSprite = arrowImages[index].sprite;
        foreach (var arrowData in arrowsPrefab)
        {
            if (arrowData.ArrowImage.sprite == currentSprite)
            {
                return arrowData.ArrowKeyCode;
            }
        }

        return KeyCode.None; // Если не найдено совпадений
    }

    private void ResetGame()
    {
        currentArrowIndex = 0;
        timer = timeLimit;

        foreach (var arrow in arrowImages)
        {
            if (arrow.gameObject.activeSelf)
            {
                arrow.color = grayColor;
            }
        }
    }

    private float CalculateProgress(float correctRatio, float currentCount, float totalCount, float penalty)
    {
        float progress;
        correctRatio = currentCount / totalCount;

        if (_isPerfect)
        {
            return progress = correctRatio * 1.5f;
        }

        if (correctRatio >= 1f && !_isPerfect)
        {
            return progress = correctRatio;
        }

        if (correctRatio >= 0.9f)
        {
            return progress = correctRatio * 0.75f;
        }

        if (correctRatio >= 0.75f)
        {
            return progress = correctRatio * 0.5f;
        }

        if (correctRatio >= 0.5f)
        {
            return progress = 0;
        }

        if (correctRatio >= 0.25f)
        {
            return progress = -penalty * 0.5f;
        }

        else
        {
            return progress = -penalty;
        }
    }

    private bool IsTimeLeft()
    {
        if (FishingTimer.Instance.IsPenalty())
        {
            FishingProgress.Instance.AddToProgressBar(_addedProgress);
            
            if (FishingProgress.Instance.IsNotFilled())
            {
                EndGame(false);
                return true;
            }

            // Неправильное нажатие: меняем все цвета на красные
            foreach (var arrow in arrowImages)
            {
                if (arrow.gameObject.activeSelf)
                {
                    arrow.color = redColor;
                }
            }

            _currentCount = 0;
            _isPerfect = false;

            // Сбрасываем прогресс
            StartCoroutine(ResetAfterError());
            ResetGame();
            FishingProgress.Instance.ResetProgress();
            FishingTimer.Instance.ResetTimer();

            return true;
        }

        return false;
    }

    // Завершение мини-игры
    private void EndGame(bool success)
    {
        isPlaying = false;
        fishingGameUI.SetActive(false);
        FishingProgress.Instance.gameObject.SetActive(false);
        FishingTimer.Instance.gameObject.SetActive(false);
        StopCoroutine(FishingTimer.Instance.StartTimer());

        if (success)
        {
            // StartCoroutine(ResetAfterWin());
            // FishingIsCatched.Instance.HideWin();
            Debug.Log("Мини игра пройдена!");
        }
        else
        {
            // StartCoroutine(ResetAfterLose());
            // FishingIsCatched.Instance.HideLose();
            Debug.Log("Мини игра провалена!");
        }
    }

    // Сброс прогресса после ошибки
    private IEnumerator ResetAfterError()
    {
        yield return new WaitForSeconds(0.5f); // Задержка для визуального эффекта

        foreach (var arrow in arrowImages)
        {
            if (arrow.gameObject.activeSelf)
            {
                arrow.color = grayColor;
            }
        }

        currentArrowIndex = 0;
    }

    private IEnumerator ResetAfterLose()
    {
        FishingIsCatched.Instance.ShowLose();
        yield return new WaitForSeconds(2f);
    }
    
    private IEnumerator ResetAfterWin()
    {
        FishingIsCatched.Instance.ShowWin();
        yield return new WaitForSeconds(2f);
    }
}