using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected float damage = 50f;
    [SerializeField] protected LayerMask affectedLayers;
    
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected float tickRate = 1f;

    protected virtual void Start()
    {
        StartCoroutine(TickRoutine());
    }
    
    protected IEnumerator TickRoutine()
    {
        float timePassed = 0f;
        float tickInterval = 1f / tickRate;
        
        // First tick happens immediately
        ApplyExplosionEffect();
        
        // Continue ticking until duration is over
        while (timePassed < duration)
        {
            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
            
            // Apply effect at each tick
            ApplyExplosionEffect();
        }
        
        // Destroy after duration
        Destroy(gameObject);
    }
    
    protected virtual void ApplyExplosionEffect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, affectedLayers);
        
        foreach (var hit in hits)
        {
            ProcessTarget(hit);
        }
    }
    
    protected virtual void ProcessTarget(Collider2D hit)
    {
        IDamageable target = hit.GetComponent<IDamageable>();
        if (target != null)
        {
            // Get the closest point on the collider to the explosion
            Vector2 closestPoint = hit.ClosestPoint(transform.position);
            float distance = Vector2.Distance(transform.position, closestPoint);
        
            float damageFactor = 1f - Mathf.Clamp01(distance / radius);
            float finalDamage = damage * damageFactor;
        
            target.GetDamage(finalDamage);
        }
    }

    protected virtual void OnDestroy()
    {
        Debug.Log("Explosion effect was destroyed");
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}