using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel; // Panel с контентом
    [SerializeField] private Image noteImage; // Изображение записки
    [SerializeField] private TextMeshProUGUI noteText; // Текст записки

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
        if (noteItem == null) return;

        notePanel.SetActive(true);
        noteImage.sprite = noteItem.itemIcon; // Устанавливаем иконку записки
        noteText.text = noteItem.content;
    }

    public void HideNote()
    {
        notePanel.SetActive(false);
    }
}