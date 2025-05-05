using UnityEngine;

public class PoisonExplosion : Explosion 
{
   [SerializeField] private GameObject poisonEffectPrefab;
    
   private GameObject visualEffect;
   
   protected override void ProcessTarget(Collider2D hit)
   {
      Enemy enemy = hit.GetComponent<Enemy>();
      if (enemy != null)
      {
         Vector2 closestPoint = hit.ClosestPoint(transform.position);
         float distance = Vector2.Distance(transform.position, closestPoint);
        
         float damageFactor = 1f - Mathf.Clamp01(distance / radius);
        
         // Calculate damage per tick based on total potential ticks
         float damagePerTick = damage * damageFactor / (duration * tickRate);
        
         enemy.GetDamage(damagePerTick);
      }
   }
    
   protected override void OnDestroy()
   {
      if (visualEffect != null)
         Destroy(visualEffect);
   }
}