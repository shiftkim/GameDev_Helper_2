using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Launcher : MonoBehaviour
{
   [SerializeField] private Camera mainCamera;
   [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
   [SerializeField] private float fireRate = 0.5f;
   
   [Header("Projectiles")]
   [SerializeField] private List<GameObject> projectilePrefabs = new List<GameObject>();
   
   [Header("Horizontal Movement")]
   [SerializeField] private float minSpawnX = -8f;
   [SerializeField] private float maxSpawnX = 8f;
   // [SerializeField] private bool visualizeHorizontalRange = true;
   
   private GameObject projectilePrefab;
   private float nextFireTime;
   private SpriteRenderer spriteRenderer;
   private float fixedYPosition;
   private int currentProjectileIndex = 0;
   
   private void Awake()
   {
       spriteRenderer = GetComponent<SpriteRenderer>();
       if (spriteRenderer == null)
           spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
           
       if (projectilePrefabs.Count > 0)
       {
           currentProjectileIndex = 0;
           projectilePrefab = projectilePrefabs[currentProjectileIndex];
       }
   }
   
   private void Start()
   {
       if (mainCamera == null)
           mainCamera = Camera.main;
       
       fixedYPosition = transform.position.y;
       UpdateLauncherSprite();
   }
   
   private void Update()
   {
       Vector3 mouseWorldPosition = GetMouseWorldPosition();
       float restrictedX = Mathf.Clamp(mouseWorldPosition.x, minSpawnX, maxSpawnX);
       transform.position = new Vector3(restrictedX, fixedYPosition, transform.position.z);
       
       // Handle mouse scroll for projectile switching
       float scrollDelta = Input.GetAxisRaw("Mouse ScrollWheel");
       if (scrollDelta != 0 && projectilePrefabs.Count > 0)
       {
           if (scrollDelta > 0)
               currentProjectileIndex = (currentProjectileIndex + 1) % projectilePrefabs.Count;
           else if (scrollDelta < 0)
               currentProjectileIndex = (currentProjectileIndex - 1 + projectilePrefabs.Count) % projectilePrefabs.Count;
           
           projectilePrefab = projectilePrefabs[currentProjectileIndex];
           UpdateLauncherSprite();
       }
       
       if (Input.GetKey(fireKey) && Time.time >= nextFireTime && !IsPointerOverUI())
       {
           FireProjectile(transform.position);
           nextFireTime = Time.time + fireRate;
       }
   }
   
   private bool IsPointerOverUI() => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
   
   private Vector3 GetMouseWorldPosition()
   {
       Vector3 mousePos = Input.mousePosition;
       mousePos.z = 10; 
       return mainCamera.ScreenToWorldPoint(mousePos);
   }
   
   // ReSharper disable Unity.PerformanceAnalysis
   private void FireProjectile(Vector3 position)
   {
       if (projectilePrefab != null)
           Instantiate(projectilePrefab, position, Quaternion.identity);
       else
           Debug.LogError("No projectile prefabs assigned to Launcher");
   }
   
   // ReSharper disable Unity.PerformanceAnalysis
   private void UpdateLauncherSprite()
   {
       if (projectilePrefab != null && spriteRenderer != null)
       {
           SpriteRenderer projectileRenderer = projectilePrefab.GetComponent<SpriteRenderer>() 
               ?? projectilePrefab.GetComponentInChildren<SpriteRenderer>();
           
           if (projectileRenderer != null && projectileRenderer.sprite != null)
           {
               spriteRenderer.sprite = projectileRenderer.sprite;
               spriteRenderer.color = projectileRenderer.color;
           }
       }
   }
}

// private void OnDrawGizmos()
// {
//     if (visualizeHorizontalRange)
//     {
//         Gizmos.color = Color.yellow;
//         float yPos = Application.isPlaying ? fixedYPosition : transform.position.y;
//         Vector3 leftPoint = new Vector3(minSpawnX, yPos, 0);
//         Vector3 rightPoint = new Vector3(maxSpawnX, yPos, 0);
//            
//         Gizmos.DrawLine(leftPoint, rightPoint);
//         Gizmos.DrawLine(leftPoint, leftPoint + Vector3.up * 0.5f);
//         Gizmos.DrawLine(rightPoint, rightPoint + Vector3.up * 0.5f);
//     }
// }