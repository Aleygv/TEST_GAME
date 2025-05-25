using UnityEngine;
using UnityEngine.InputSystem;

public class EntryPoint : MonoBehaviour
{
    // [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _visualPrefab;

    private void Awake()
    {
        
        // 1. ������� ��������� ������ ������ 
        GameObject playerSystem = new GameObject("PlayerSystem");
        playerSystem.AddComponent<Animator>();
        playerSystem.AddComponent<PlayerInput>();
        playerSystem.AddComponent<FishingZone>();
        // playerSystem.AddComponent<PlayerData>();

        // 2. ��������� ���������� ����������
        var movement = playerSystem.AddComponent<PlayerMovement>();

        // 3. ������� ������ ��������
        GameObject visual = Instantiate(_visualPrefab);
        visual.transform.SetParent(playerSystem.transform);

        // 4. ��������� ����������
        
        movement.Init(visual.GetComponent<Animator>());

    }
}