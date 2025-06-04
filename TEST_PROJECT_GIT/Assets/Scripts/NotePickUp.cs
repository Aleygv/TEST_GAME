using UnityEngine;

public class NotePickUp : MonoBehaviour
{
    [SerializeField] private NoteItem noteData; // Записка, которую подбираем
    private bool playerInRange;

    private void Update()
    {
        if (playerInRange)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        Inventory.instance.AddItem(noteData, 1);
        Debug.Log($"Вы подобрали записку: {noteData.name}");
        Destroy(gameObject); // Удаляем объект из мира
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}