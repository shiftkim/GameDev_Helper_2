using System;
using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText;
    
    private void Awake()
    {
        if(coinCountText == null)
            Debug.LogError("CoinsIndicator: text element is not assigned to coinCountText");
        Inventory.OnCoinsChanged += UpdateText;
    }

    private void Start()
    {
        UpdateText(Inventory.Instance.Coins);
    }

    private void OnDestroy()
    {
        Inventory.OnCoinsChanged -= UpdateText;
    }
    
    private void UpdateText(int coinsAmount)
    {
        if (coinCountText != null)
            coinCountText.text = coinsAmount.ToString();
    }
}