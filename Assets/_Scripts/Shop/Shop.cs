using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
   [SerializeField] private Transform contentRoot;
   [SerializeField] private GameObject itemButtonPrefab;
   [SerializeField] private List<ItemInfo> shopItems;
   
   private Animator _animator;
    
   private void Awake()
   {
      _animator = GetComponent<Animator>();
      foreach (var item in shopItems)
      {
         Debug.Log($"Shop creates a button for {item.name}");
         var btnGO = Instantiate(itemButtonPrefab, contentRoot);
         var btn = btnGO.GetComponent<ShopItem>();
         btn.Init(item);
         btn.OnBuyClicked += HandleBuyClicked;
      }
   }
   
   private void HandleBuyClicked(ItemInfo item)
   {
      Debug.Log($"HandleBuyClicked called for item id: {item.id}");
    
      if (Inventory.Instance.TryBuyItem(item))
      {
         Debug.Log($"Item {item.id} successfully bought, updating UI...");
        
         foreach (Transform child in contentRoot)
         {
            var shopItem = child.GetComponent<ShopItem>();
            if (shopItem != null)
            {
               Debug.Log($"Checking ShopItem with id: {shopItem.ItemId}");
               if (shopItem.ItemId == item.id)
               {
                  Debug.Log($"Found matching ShopItem {item.id}, calling SetPurchased()");
                  shopItem.SetPurchased();
                  break;
               }
            }
         }
      }
      else
      {
         Debug.Log($"Failed to buy item {item.id}, not enough coins or already purchased.");
      }
   }
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Tab))
      {
         _animator.SetTrigger("close");
      }
   }
   
   public void CloseShop()
   {
      gameObject.SetActive(false);
   }
}