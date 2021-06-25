using UnityEngine;


[CreateAssetMenu(fileName = "StoreItem", menuName = "Store/Item")]
public class StoreItem : ScriptableObject
{
    public Item itemType;
    public int cost;
    public string itemName;
    public GameObject g;
    public Sprite img;
    public int id;
    public bool isPurchased;

}
