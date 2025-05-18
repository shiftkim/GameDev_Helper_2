using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI coinCountText;

    private void Awake()
    {
        if(coinCountText == null)
            Debug.LogError("UIManager: coinCountText is null");
    }

    private void OnEnable()
    {
        Inventory.OnCoinsChanged += UpdateCoinUI;
    }

    private void OnDisable()
    {
        Inventory.OnCoinsChanged -= UpdateCoinUI;
    }

    [SerializeField] private Shop shop;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shop.gameObject.SetActive(true);
        }
    }
    
    private void UpdateCoinUI(int coinsAmount)
    {
        Debug.Log($"UIManager: UpdateCoinUI({coinsAmount})");
        if (coinCountText != null)
            coinCountText.text = coinsAmount.ToString();
    }
}
