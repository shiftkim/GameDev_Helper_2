using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Animator))]
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
      _itemInfo = itemInfo;
      icon.sprite = itemInfo.icon;
      priceText.text = itemInfo.cost.ToString();
      
      bool isPurchased = Inventory.Instance.IsItemPurchased(itemInfo.id);
      
      purchasedOverlay.SetActive(isPurchased);
      buyButton.interactable = !isPurchased;
      buyButton.onClick.AddListener(HandleClick);
   }
   
   void HandleClick()
   {
      Debug.Log($"Buy {_itemInfo.id} clicked");
      OnBuyClicked?.Invoke(_itemInfo);
   }
   
   //Дописать на втором уроке
   public void SetPurchased()
   {
      Debug.Log($"Setting purchased overlay ? {_itemInfo.id}");
      purchasedOverlay.SetActive(true);
      buyButton.interactable = false;
   }
   
   public void SetUnpurchased()
   {
      Debug.Log($"Setting unpurchased overlay for {_itemInfo.id}");
      purchasedOverlay.SetActive(false);
      buyButton.interactable = true;
   }
   
   public void TriggerNotEnoughMoney()
   {
      Animator animator = GetComponent<Animator>();
      if (animator != null)
      {
         animator.ResetTrigger("notEnoughMoney");
         animator.SetTrigger("notEnoughMoney");
      }
   }
}