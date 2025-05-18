using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public static event Action<int> OnCoinsChanged;

    private InventoryData _data;

    public int Coins => _data.coins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _data = SaveManager.LoadData();
            OnCoinsChanged?.Invoke(Coins);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Item.OnItemDestroyed += AddCoins;
    }
    
    private void OnDisable()
    {
        Item.OnItemDestroyed -= AddCoins;
    }
    
    private void OnApplicationQuit()
    {
        SaveManager.SaveData(_data);
    }

    public bool TryBuyItem(ItemInfo item)
    {
        if (_data.PurchasedItems.ContainsKey(item.id)) return false;
        if (_data.coins < item.cost) return false;

        _data.coins -= item.cost;
        _data.PurchasedItems[item.id] = true;

        SaveManager.SaveData(_data);
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }

    public bool IsItemPurchased(string id) =>
        _data.PurchasedItems.TryGetValue(id, out var purchased) && purchased;

    public void AddCoins(int amount)
    {
        _data.coins += amount;
        SaveManager.SaveData(_data);
        OnCoinsChanged?.Invoke(Coins);
    }
}


[Serializable]
public class InventoryData
{
    public int coins;
    public Dictionary<string, bool> PurchasedItems = new();
}
