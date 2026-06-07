using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Pawnshop/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int price;
    [SerializeField] private string rarity;
    [SerializeField] private bool isCursed;
    [SerializeField] private GameObject itemPrefab;

    public string ItemName => itemName;
    public int Price => price;
    public string Rarity => rarity;
    public bool IsCursed => isCursed;
    public GameObject ItemPrefab => itemPrefab;
}
