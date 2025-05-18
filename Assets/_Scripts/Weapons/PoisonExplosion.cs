using UnityEngine;

public class PoisonExplosion : Explosion 
{
    
   private GameObject visualEffect;
   
   protected override void ProcessTarget(Collider2D hit)
   {
      IDamageable target = hit.GetComponent<IDamageable>();
      if (target != null)
      {
         Vector2 closestPoint = hit.ClosestPoint(transform.position);
         float distance = Vector2.Distance(transform.position, closestPoint);
         float damageFactor = 1f - Mathf.Clamp01(distance / radius);
         float damagePerTick = damage * damageFactor / (duration * tickRate);
         target.GetDamage(damagePerTick);
      }
   }
}