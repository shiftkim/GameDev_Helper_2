using UnityEngine;

[CreateAssetMenu(fileName = "NewItemInfo", menuName = "Items/New ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public string id;
    public int coinsReward = 1;
    public float maxHealth = 100f;
    public int cost = 1;
    public Sprite icon;
}