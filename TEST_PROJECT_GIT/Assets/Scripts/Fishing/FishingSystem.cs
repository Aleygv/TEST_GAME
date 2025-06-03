using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FishingSystem : MonoBehaviour
{
    [SerializeField] private GameObject fishingGameUI; // Панель с мини-игрой
    [SerializeField] private Image[] arrowImages; // Массив изображений стрелочек

    [SerializeField] private ArrowButton[] arrowsPrefab;

    [SerializeField] private Color grayColor = Color.gray;
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private Color redColor = Color.red;

    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private float swipeCooldown = 0.5f; // Задержка между свайпами в секундах

    private int _currentArrowIndex = 0;

    private float _timer;

    private bool _isPlaying = false;

    //Прогресс бар и таймер
    private float _progress = 0.0f;
    private readonly int _circlePrice = 20;
    private readonly float _circlePenalty = 0.15f;
    private bool _isPerfect = true;
    private float _correctRatio;
    private float _currentCount = 0;
    private float _totalCount;
    private float _addedProgress;
    private float _aimToFillProgressBar;
    private int _currentRepeat = 0;
    private float _lastSwipeTime; // Время последнего свайпа
    private float swipeResistance = 5;

    private Input_presystem _inputSystem;

    void Start()
    {
        fishingGameUI.SetActive(false);
        FishingProgress.Instance.gameObject.SetActive(false);
        FishingTimer.Instance.gameObject.SetActive(false);
        // FishingIsCatched.Instance.HideWin();
        // FishingIsCatched.Instance.HideLose();

        _inputSystem = new Input_presystem();
        _inputSystem.Minigame.swipe.performed += OnSwipePerformed;
    }

    void OnDestroy()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Minigame.swipe.performed -= OnSwipePerformed;
            _inputSystem.Dispose();
        }
    }

    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        if (_isPlaying)
        {
            // Проверяем, прошло ли достаточно времени с последнего свайпа
            if (Time.time - _lastSwipeTime < swipeCooldown)
            {
                return;
            }

            Vector2 swipeDirection = context.ReadValue<Vector2>();
            Debug.Log($"Raw swipe: X={swipeDirection.x}, Y={swipeDirection.y}");

            // Определяем направление на основе большей компоненты
            Vector2 resultDirection;
            
            if (Mathf.Abs(swipeDirection.x) > 0.5f || Mathf.Abs(swipeDirection.y) > 0.5f)
            {
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    resultDirection = swipeDirection.x > 0 ? Vector2.right : Vector2.left;
                    Debug.Log($"Horizontal swipe detected: {(swipeDirection.x > 0 ? "Right" : "Left")}");
                }
                else
                {
                    resultDirection = swipeDirection.y > 0 ? Vector2.up : Vector2.down;
                    Debug.Log($"Vertical swipe detected: {(swipeDirection.y > 0 ? "Up" : "Down")}");
                }

                _lastSwipeTime = Time.time;
                CheckArrow(resultDirection);
            }
        }
    }

    void Update()
    {
        if (_isPlaying)
        {
            if (IsTimeLeft())
            {
                if (FishingProgress.Instance.IsNotFilled())
                {
                    EndGame(false);
                    FishingProgress.Instance.ResetProgress();
                }
                if (FishingProgress.Instance.IsFillOver())
                {
                    EndGame(true);
                    FishingProgress.Instance.ResetProgress();
                }
            }
        }
    }

    public void StartMiniGame()
    {
        if (!_isPlaying)
        {
            GameManager.StartFishing();
            _lastSwipeTime = Time.time; // Инициализируем время последнего свайпа

            fishingGameUI.SetActive(true);
            FishingProgress.Instance.gameObject.SetActive(true);
            FishingTimer.Instance.gameObject.SetActive(true);

            _inputSystem.Minigame.Enable();
            StartNewRound();

            StartCoroutine(FishingTimer.Instance.StartTimer());
            _isPlaying = true;
            _currentRepeat = 0;
            _aimToFillProgressBar = Random.Range(4f, 9f);
        }
    }

    private ArrowButton[] GenerateRandomArrowSequence(int count)
    {
        ArrowButton[] sequence = new ArrowButton[count];
        for (int i = 0; i < count; i++)
        {
            sequence[i] = arrowsPrefab[Random.Range(0, arrowsPrefab.Length)];
        }
        return sequence;
    }
    
    private void StartNewRound()
    {
        _currentArrowIndex = 0;
        _totalCount = Random.Range(3, 6);
        ArrowButton[] sequence = GenerateRandomArrowSequence((int)_totalCount);

        // Отключаем все стрелки
        foreach (var arrow in arrowImages)
        {
            arrow.gameObject.SetActive(false);
        }

        // Активируем и настраиваем нужные стрелки
        for (int i = 0; i < sequence.Length; i++)
        {
            arrowImages[i].gameObject.SetActive(true);
            arrowImages[i].sprite = sequence[i].ArrowImage.sprite;
            arrowImages[i].color = grayColor;
        }

        ResetGame();
    }

    private void CheckArrow(Vector2 swipeDirection)
    {
        if (_currentArrowIndex >= arrowImages.Length || !arrowImages[_currentArrowIndex].gameObject.activeSelf)
            return;

        Vector2 expectedDirection = GetExpectedDirection(_currentArrowIndex);
        
        // Добавляем отладочную информацию
        Debug.Log($"Swipe Direction: {swipeDirection}, Expected Direction: {expectedDirection}");
        
        // Сравниваем направления с учетом погрешности
        bool isCorrectDirection = false;
        
        if (expectedDirection == Vector2.up && swipeDirection.y > 0.5f)
            isCorrectDirection = true;
        else if (expectedDirection == Vector2.down && swipeDirection.y < -0.5f)
            isCorrectDirection = true;
        else if (expectedDirection == Vector2.left && swipeDirection.x < -0.5f)
            isCorrectDirection = true;
        else if (expectedDirection == Vector2.right && swipeDirection.x > 0.5f)
            isCorrectDirection = true;

        if (isCorrectDirection)
        {
            Debug.Log("Correct direction!");
            arrowImages[_currentArrowIndex].color = whiteColor;
            _currentArrowIndex++;
            _currentCount++;

            if (_currentArrowIndex >= arrowImages.Length || !arrowImages[_currentArrowIndex].gameObject.activeSelf)
            {
                FishingTimer.Instance.ResetTimer();
                _addedProgress = CalculateProgress(_correctRatio, _currentCount, _totalCount, 0.15f) / _aimToFillProgressBar;
                FishingProgress.Instance.AddToProgressBar(_addedProgress);

                if (FishingProgress.Instance.IsFillOver())
                {
                    EndGame(true);
                    FishingProgress.Instance.ResetProgress();
                }
                else
                {
                    StartNewRound();
                    _isPerfect = true;
                    _currentCount = 0;
                }
            }
        }
        else
        {
            
            foreach (var arrow in arrowImages)
            {
                if (arrow.gameObject.activeSelf)
                {
                    arrow.color = redColor;
                }
            }

            _currentCount = 0;
            _isPerfect = false;
            StartCoroutine(ResetAfterError());
        }
    }

    private Vector2 GetExpectedDirection(int index)
    {
        Sprite currentSprite = arrowImages[index].sprite;
        foreach (var arrowData in arrowsPrefab)
        {
            if (arrowData.ArrowImage.sprite == currentSprite)
            {
                return arrowData.Direction;
            }
        }
        return Vector2.zero;
    }

    private void ResetGame()
    {
        _currentArrowIndex = 0;
        _timer = timeLimit;

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
        correctRatio = currentCount / totalCount;

        if (_isPerfect)
        {
            return correctRatio * 1.5f;
        }

        if (correctRatio >= 1f && !_isPerfect)
        {
            return correctRatio;
        }

        return 0;
    }

    private bool IsTimeLeft()
    {
        if (FishingTimer.Instance.IsPenalty())
        {
            _currentCount = 0;
            _isPerfect = false;

            ResetGame();
            FishingProgress.Instance.AddToProgressBar(-_circlePenalty);
            FishingTimer.Instance.ResetTimer();

            return true;
        }

        return false;
    }

    // Завершение мини-игры
    private void EndGame(bool success)
    {
        _isPlaying = false;
        _inputSystem.Minigame.Disable();
        fishingGameUI.SetActive(false);
        FishingProgress.Instance.gameObject.SetActive(false);
        FishingTimer.Instance.gameObject.SetActive(false);
        StopCoroutine(FishingTimer.Instance.StartTimer());

        //After conditions you can change scheme to PlayerInput
        if (success)
        {
            Debug.Log("Мини игра пройдена!");
            GameManager.FishingWin();
            FishGenerator.CaughtFish();
            GameManager.StopFishing();
        }
        else
        {
            Debug.Log("Мини игра провалена!");
            GameManager.FishingLose();
            GameManager.StopFishing();
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

        _currentArrowIndex = 0;
    }
}