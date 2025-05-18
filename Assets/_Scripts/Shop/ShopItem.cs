using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItem : MonoBehaviour
{
   public event Action<ItemInfo> OnBuyClicked;

   [SerializeField] private Image icon;
   [SerializeField] private TextMeshProUGUI priceText;
   [SerializeField] private Button buyButton;
   [SerializeField] private GameObject purchasedOverlay;

   private ItemInfo _itemInfo;
   public string ItemId => _itemInfo.id;


   public void Init(ItemInfo itemInfo)
   {
      Debug.Log($"Initializing item {itemInfo.id} with cost {itemInfo.cost}");
      _itemInfo = itemInfo;
      icon.sprite = itemInfo.icon;
      priceText.text = itemInfo.cost.ToString();

      Debug.Log($"Button for {itemInfo.id} is set, checking for purchased state");
      bool isPurchased = Inventory.Instance.IsItemPurchased(itemInfo.id);
      
      Debug.Log($"Setting is purchased overlay ? {isPurchased}");
      purchasedOverlay.SetActive(isPurchased);
      buyButton.interactable = !isPurchased;
      buyButton.onClick.AddListener(HandleClick);
   }
   
   void HandleClick()
   {
      Debug.Log($"Buy {_itemInfo.id} clicked");
      OnBuyClicked?.Invoke(_itemInfo);
   }
   
   public void SetPurchased()
   {
      Debug.Log($"Setting purchased overlay ? {_itemInfo.id}");
      purchasedOverlay.SetActive(true);
      buyButton.interactable = false;
   }
}