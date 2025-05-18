using UnityEngine;

public class BlastExplosion : Explosion
{
   [SerializeField] private float explosionForce = 700f;
   [SerializeField] private float upwardsModifier = 3f;
   [SerializeField] private float torqueForce = 2000f;

   protected override void ProcessTarget(Collider2D hit)
   {
      base.ProcessTarget(hit);
    
      Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
      if (rb != null)
      {
         Vector2 closestPoint = hit.ClosestPoint(transform.position);
         Vector2 direction = hit.transform.position - transform.position;
        
         float distance = Vector2.Distance(transform.position, closestPoint);
         float forceFactor = 1f - Mathf.Clamp01(distance / radius);
        
         Vector2 forceDirection = direction.normalized;
         forceDirection.y += upwardsModifier * forceFactor;
         forceDirection.Normalize();
        
         Vector2 force = forceDirection * (explosionForce * forceFactor);
         rb.AddForce(force, ForceMode2D.Impulse);
        
         float randomSign = Random.Range(0, 2) * 2 - 1;
         float rotationalForce = randomSign * torqueForce * forceFactor;
         rb.AddTorque(rotationalForce, ForceMode2D.Impulse);
      }
   }
}