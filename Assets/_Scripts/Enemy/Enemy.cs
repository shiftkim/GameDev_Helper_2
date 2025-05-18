using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour, IDamageable
{
   [SerializeField] protected EnemyData enemyData;
   protected float currentHealth;
   public static event Action<Enemy> OnEnemyDeath;
   public float CurrentHealth 
   { 
      get { return currentHealth; }
      protected set { currentHealth = value; }
   }
   
   public virtual void GetDamage(float damage)
   {
      currentHealth -= damage;
      Debug.Log($"{enemyData.enemyName} got {damage} damage and health now is {currentHealth}");
        
      if (currentHealth <= 0)
      {
         Die();
      }
   }
   
   protected virtual void Awake()
   {
      if (enemyData != null)
      {
         InitializeFromData();
      }
      else
      {
         Debug.LogError("Enemy Data not assigned to " + gameObject.name);
      }
   }
    
   protected virtual void InitializeFromData()
   {
      currentHealth = enemyData.maxHealth;
   }

   public virtual void OnSpawn()
   {
      Debug.Log($"'{enemyData.enemyName}' spawned on Scene");
   }
    
   public virtual void Die()
   {
      Debug.Log($"'{enemyData.enemyName}' is dying");
        
      // Trigger the death event
      OnEnemyDeath?.Invoke(this);
        
      Destroy(gameObject);
   }

   
    
   // Public method to set enemy data at runtime (used by spawner)
   public void SetEnemyData(EnemyData data)
   {
      enemyData = data;
      InitializeFromData();
   }

  
}