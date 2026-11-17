using UnityEngine;

public enum TrashCategory
{
    Organic,
    NonOrganic,
    B3
}

public class TrashItem : MonoBehaviour
{
    [Header("Informasi Sampah")]
    public string trashName;
    public TrashCategory category;
}