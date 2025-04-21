using UnityEngine;

public class Bomb : MonoBehaviour
{
   [SerializeField] private float explosionRadius = 5f;
   [SerializeField] private float explosionForce = 700f;
   [SerializeField] private float upwardsModifier = 3f;
   [SerializeField] private float torqueForce = 2000f;
   [SerializeField] private float explosionDamage = 50f;
   
   [Header("Layer Settings")]
   [SerializeField] private LayerMask affectedLayers;    // Layers that receive explosion forces/damage
   [SerializeField] private LayerMask triggerLayers;     // Layers that can trigger the explosion
   [SerializeField] private GameObject sparkEffectPrefab;

   void OnCollisionEnter2D(Collision2D col)
   {
      // Check if the collision is with a layer that should trigger explosion
      if (((1 << col.gameObject.layer) & triggerLayers) != 0)
      {
         Explode();
      }
   }

   void Explode()
   {
      Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);
      
      foreach (Collider2D hit in colliders)
      {
         ApplyPhysicsForces(hit);
         ApplyDamage(hit);
      }
      
      CreateExplosionEffect();
      Destroy(gameObject);
   }
   
   void ApplyPhysicsForces(Collider2D hit)
   {
      Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
      if (rb == null) return;
      
      Vector2 direction = hit.transform.position - transform.position;
      float distance = direction.magnitude;
      float forceFactor = 1f - Mathf.Clamp01(distance / explosionRadius);
      
      // Apply directional force
      Vector2 forceDirection = direction.normalized;
      forceDirection.y += upwardsModifier * forceFactor;
      forceDirection.Normalize();
      
      Vector2 force = forceDirection * explosionForce * forceFactor;
      rb.AddForce(force, ForceMode2D.Impulse);
      
      // Apply torque
      float randomSign = Random.Range(0, 2) * 2 - 1;
      float rotationalForce = randomSign * torqueForce * forceFactor;
      rb.AddTorque(rotationalForce, ForceMode2D.Impulse);
   }
   
   void ApplyDamage(Collider2D hit)
   {
      Enemy enemy = hit.GetComponent<Enemy>();
      if (enemy == null) return;
      
      float distance = Vector2.Distance(transform.position, hit.transform.position);
      float damageFactor = 1f - Mathf.Clamp01(distance / explosionRadius);
      float damage = explosionDamage * damageFactor;
      
      enemy.GetDamage(damage);
   }
   
   void CreateExplosionEffect()
   {
      if (sparkEffectPrefab != null)
      {
         Instantiate(sparkEffectPrefab, transform.position, Quaternion.identity);
      }
   }
    
   void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, explosionRadius);
   }
}