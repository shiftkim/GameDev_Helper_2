using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Shop shop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shop.gameObject.SetActive(true);
        }
    }
    
    private void Awake()
    {
        if(shop == null)
            Debug.LogError("UIManager: Shop is null");
    }
}
