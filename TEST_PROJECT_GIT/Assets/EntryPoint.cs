using UnityEngine;
using UnityEngine.InputSystem;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _visualPrefab;
    
    

    private void Awake()
    {

        
        // 1. creating system object of player
        GameObject playerSystem = new GameObject("PlayerSystem");
        playerSystem.AddComponent<Animator>();

        // 2. adding logic components
        var movement = playerSystem.AddComponent<PlayerMovement>();

        

        // 3.making visual separetly
        GameObject visual = Instantiate(_visualPrefab);
        visual.transform.SetParent(playerSystem.transform);
        


        // 4. tying them together
        
        movement.Init(visual.GetComponent<Animator>());
        


    }
}