using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private InventoryData _data;
    public static Inventory Instance { get; private set; }
    public static event Action<int> OnCoinsChanged;
    public int Coins => _data.coins;

    //Дописать на 2 уроке
    public void ResetShop()
    {
        Debug.Log("Resetting shop - clearing all purchases");
        _data.PurchasedItems.Clear();
        _data.coins = 0;
        SaveManager.SaveData(_data);
        OnCoinsChanged?.Invoke(Coins);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _data = SaveManager.LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
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
    
    
    private void OnEnable()
    {
        OnCoinsChanged?.Invoke(Coins);
        Item.OnItemDestroyed += AddCoins;
    }
    
    private void OnDisable()
    {
        Item.OnItemDestroyed -= AddCoins;
    }
    
    public void AddCoins(int amount)
    {
        _data.coins += amount;
        SaveManager.SaveData(_data);
        OnCoinsChanged?.Invoke(Coins);
    }
    
    private void OnApplicationQuit()
    {
        SaveManager.SaveData(_data);
    }
    
}



