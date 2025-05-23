using UnityEngine;
using UnityEngine.InputSystem;

public class EntryPoint : MonoBehaviour
{
    // [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _visualPrefab;

    private void Awake()
    {
        
        // 1. Создаем системный объект игрока 
        GameObject playerSystem = new GameObject("PlayerSystem");
        playerSystem.AddComponent<Animator>();
        playerSystem.AddComponent<PlayerInput>();
        playerSystem.AddComponent<FishingZone>();
        // playerSystem.AddComponent<PlayerData>();

        // 2. Добавляем логические компоненты
        var movement = playerSystem.AddComponent<PlayerMovement>();

        // 3. Создаем визуал отдельно
        GameObject visual = Instantiate(_visualPrefab);
        visual.transform.SetParent(playerSystem.transform);

        // 4. Связываем компоненты
        
        movement.Init(visual.GetComponent<Animator>());

    }
}