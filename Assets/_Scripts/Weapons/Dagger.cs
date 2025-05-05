using UnityEngine;

public class Dagger : Throwable
{
   [SerializeField] private LayerMask triggerLayers;
   
   public override void OnCollisionEnter2D(Collision2D col)
   {
      if (((1 << col.gameObject.layer) & triggerLayers) != 0)
      {
         CreateEffect();
         Destroy(gameObject);
      }
   }
}