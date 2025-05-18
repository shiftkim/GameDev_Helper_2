using UnityEngine;
using System;
using System.Collections;

public class Item : MonoBehaviour, IDamageable
{
    public ItemInfo itemInfo;
    private float currentHealth;
    public static event Action<int> OnItemDestroyed;
    
    [SerializeField] private float hurtEffectDuration = 0.25f;
    [SerializeField] private Color hurtColor = Color.red;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine hurtEffectCoroutine;
    
    protected virtual void Break()
    {
        Debug.Log($"item {itemInfo.id} has been destroyed");
        OnItemDestroyed?.Invoke(itemInfo.coinsReward);
        Destroy(gameObject);
    }
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on Item: " + gameObject.name);
        }
    }
    
    
    
    void Start()
    {
        currentHealth = itemInfo.maxHealth;
        Debug.Log($"item {itemInfo.id} has been initialized");
    }
    
    public void GetDamage(float damage)
    {
        Debug.Log($"item {itemInfo.id} took damage {damage} health now is {currentHealth}");
        currentHealth -= damage;
        
        PlayHurtEffect();
        
        if (currentHealth <= 0)
        {
            Break();
        }
    }
    
    private void PlayHurtEffect()
    {
        if (hurtEffectCoroutine != null)
        {
            StopCoroutine(hurtEffectCoroutine);
        }
        
        if (spriteRenderer != null)
        {
            hurtEffectCoroutine = StartCoroutine(HurtEffectRoutine());
        }
    }
    
    private IEnumerator HurtEffectRoutine()
    {
        spriteRenderer.color = hurtColor;
        float elapsedTime = 0f;
        while (elapsedTime < hurtEffectDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / hurtEffectDuration;
            spriteRenderer.color = Color.Lerp(hurtColor, originalColor, t);
            yield return null;
        }
        spriteRenderer.color = originalColor;
        hurtEffectCoroutine = null;
    }
}