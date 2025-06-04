using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel; // Главная панель
    [SerializeField] private Image displayImage; // Полное изображение записи
    [SerializeField] private TextMeshProUGUI contentText; // Текст записи

    private static NoteUIManager _instance;
    public static NoteUIManager Instance => _instance;

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
        }

        HideNote();
    }

    public void ShowNote(NoteItem noteItem)
    {
        if (noteItem == null)
        {
            Debug.LogError("NoteItem не может быть null");
            return;
        }

        notePanel.SetActive(true);

        // Показываем изображение
        if (noteItem.itemIcon != null)
        {
            // displayImage.sprite = noteItem.itemIcon;
            displayImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("У записи нет изображения.");
            displayImage.enabled = false;
        }

        // Показываем текст
        contentText.text = noteItem.content;
    }

    public void HideNote()
    {
        notePanel.SetActive(false);
    }
}