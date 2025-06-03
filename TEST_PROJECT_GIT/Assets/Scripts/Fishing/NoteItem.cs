using UnityEngine;

[CreateAssetMenu(fileName = "Note", menuName = "Item/Note")]
public class NoteItem : Item
{
    public string content;

    public override void Use()
    {
        Debug.Log("Читается заметка");
    }
}
