using UnityEngine;

public class ShowNeededWindow : MonoBehaviour
{
    [SerializeField] private GameObject firstObject;

    [Header("Optional Objects")]
    [SerializeField] private GameObject secondObject;
    [SerializeField] private GameObject thirdObject;

    public void ShowWindow(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void HideWindow(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}