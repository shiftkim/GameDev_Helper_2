using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
   [SerializeField] protected EnemyData enemyData;
   protected float currentHealth;
    
   // Event that will be triggered when the enemy dies
   public static event Action<Enemy> OnEnemyDeath;
    
   // Public property for external access
   public float CurrentHealth 
   { 
      get { return currentHealth; }
      protected set { currentHealth = value; }
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

   public virtual void GetDamage(float damage)
   {
      currentHealth -= damage;
      Debug.Log($"{enemyData.enemyName} got {damage} damage and health now is {currentHealth}");
        
      if (currentHealth <= 0)
      {
         Die();
      }
   }
    
   // Public method to set enemy data at runtime (used by spawner)
   public void SetEnemyData(EnemyData data)
   {
      enemyData = data;
      InitializeFromData();
   }
}