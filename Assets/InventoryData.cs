using System;
using System.Collections.Generic;

[Serializable]
public class InventoryData
{
    public int coins;
    public Dictionary<string, bool> PurchasedItems = new();
}