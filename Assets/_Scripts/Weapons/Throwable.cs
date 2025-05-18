using System;
using UnityEngine;

public class Throwable : MonoBehaviour
{
  [SerializeField] private InteractionLayersConfig layerConfig;
  [SerializeField] private GameObject explosionEffect;

  private void Start()
  {
    Debug.Log("Throwable instantiated");
  }

  public virtual void OnCollisionEnter2D(Collision2D col)
  {
    if (((1 << col.gameObject.layer) & layerConfig.triggerLayers) != 0)
    {
      Explode(col);
    }
  }

  protected virtual void Explode(Collision2D col)
  {
    Destroy(gameObject);
  }

  protected virtual void CreateEffect()
  {
    if (explosionEffect != null)
    {
      Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }
  }
}
