using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyData> availableEnemies = new List<EnemyData>();
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private float minSpawnDistance = 1.5f;
    [SerializeField] private LayerMask enemyLayerMask;   
    
    private Vector2 GetRandomPositionInBox()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnRandomEnemy();
        }
    }
   
    private void SpawnRandomEnemy()
    {
        if (availableEnemies.Count == 0 || spawnArea == null)
            return;
            
        EnemyData randomEnemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];
        Vector2 randomPosition = GetValidSpawnPosition();
        
        if (randomEnemyData.enemyPrefab != null)
        {
            GameObject enemyInstance = Instantiate(randomEnemyData.enemyPrefab, randomPosition, Quaternion.identity);
            Enemy enemyComponent = enemyInstance.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetEnemyData(randomEnemyData);
                enemyComponent.OnSpawn();
            }
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        int maxAttempts = 30;
        Vector2 position;
        bool isValid = false;
    
        do {
            position = GetRandomPositionInBox();
            Collider2D overlap = Physics2D.OverlapCircle(position, minSpawnDistance/2, enemyLayerMask);
            isValid = overlap == null;
            maxAttempts--;
        } while (!isValid && maxAttempts > 0);
    
        return position;
    }
    
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += HandleEnemyDeath;
    }
    
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }
    
    private void HandleEnemyDeath(Enemy enemy)
    {
        SpawnRandomEnemy();
    }
}