using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/New enemy")]
public class EnemyData : ScriptableObject
{
   [Header("Basic Stats")]
   public string enemyName = "Enemy";
   public float maxHealth = 100f;
    
   [Header("Visuals")]
   public GameObject enemyPrefab;
    
   [Header("Dropped rewards")]
   public int currencyReward = 5;
}