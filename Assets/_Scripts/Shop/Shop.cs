using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
   [SerializeField] private Transform contentRoot;
   [SerializeField] private GameObject itemButtonPrefab;
   [SerializeField] private List<ItemInfo> shopItems;
   [SerializeField] private Animator _animator;
    
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
      if (Inventory.Instance.TryBuyItem(item))
      {
         foreach (Transform child in contentRoot)
         {
            var shopItem = child.GetComponent<ShopItem>();
            if (shopItem == null || shopItem.ItemId != item.id) continue;
            shopItem.SetPurchased();
            break;
         }
      }
      else
      {
         foreach (Transform child in contentRoot)
         {
            var shopItem = child.GetComponent<ShopItem>();
            if (shopItem == null || shopItem.ItemId != item.id) continue;
            shopItem.TriggerNotEnoughMoney();
            break;
         }
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
   
   //Дописать на 2 уроке
   public void ResetShopView()
   {
      Debug.Log("Shop reset requested");
      Inventory.Instance.ResetShop();
    
      foreach (Transform child in contentRoot)
      {
         var shopItem = child.GetComponent<ShopItem>();
         if (shopItem != null)
         {
            shopItem.SetUnpurchased();
         }
      }
   }
}