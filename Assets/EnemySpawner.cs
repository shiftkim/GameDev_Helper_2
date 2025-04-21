using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyData> availableEnemies = new List<EnemyData>();
    [SerializeField] private int enemyCount = 6;
    
    private BoxCollider2D spawnArea;
    
    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        if (spawnArea == null)
        {
            Debug.LogError("EnemySpawner requires a BoxCollider2D component");
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to enemy death events
        Enemy.OnEnemyDeath += HandleEnemyDeath;
    }
    
    private void OnDisable()
    {
        // Unsubscribe when disabled or destroyed
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }
    
    private void Start()
    {
        // Spawn initial enemies
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnRandomEnemy();
        }
    }
    
    private void HandleEnemyDeath(Enemy enemy)
    {
        // Spawn a new enemy to replace the one that died
        SpawnRandomEnemy();
    }
    
    private void SpawnRandomEnemy()
    {
        if (availableEnemies.Count == 0 || spawnArea == null)
            return;
            
        // Get random enemy data
        EnemyData randomEnemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];
        
        // Get random position within the box collider
        Vector2 randomPosition = GetRandomPositionInBox();
        
        // Instantiate the enemy
        if (randomEnemyData.enemyPrefab != null)
        {
            GameObject enemyInstance = Instantiate(randomEnemyData.enemyPrefab, randomPosition, Quaternion.identity);
            
            // Assign the data to the enemy
            Enemy enemyComponent = enemyInstance.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetEnemyData(randomEnemyData);
                enemyComponent.OnSpawn();
            }
        }
    }
    
    private Vector2 GetRandomPositionInBox()
    {
        // Get the bounds of the box collider
        Bounds bounds = spawnArea.bounds;
        
        // Generate random position within these bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        
        return new Vector2(x, y);
    }
}