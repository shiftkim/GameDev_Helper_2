using UnityEngine;
using UnityEngine.EventSystems;

public class Launcher : MonoBehaviour
{
   [SerializeField] private GameObject projectilePrefab;
   [SerializeField] private Camera mainCamera;
   [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
   [SerializeField] private float fireRate = 0.5f;
   
   [Header("Spawn Restrictions")]
   [SerializeField] private float minSpawnHeight = 2f;
   [SerializeField] private float maxSpawnHeight = 10f;
   [SerializeField] private float minSpawnX = -8f;
   [SerializeField] private float maxSpawnX = 8f;
   [SerializeField] private bool visualizeSpawnArea = true;
    
   private float nextFireTime;
    
   private void Start()
   {
      if (mainCamera == null)
      {
         mainCamera = Camera.main;
      }
   }
    
   private void Update()
   {
      // Get mouse position in world space
      Vector3 mouseWorldPosition = GetMouseWorldPosition();
      
      // Restrict position based on boundaries
      Vector3 restrictedPosition = RestrictPosition(mouseWorldPosition);
      
      // Move spawner to follow cursor within restrictions
      transform.position = restrictedPosition;
        
      // Spawn from the launcher's position (not mouse position)
      if (Input.GetKey(fireKey) && Time.time >= nextFireTime && !IsPointerOverUI())
      {
         FireProjectile(transform.position);
         nextFireTime = Time.time + fireRate;
      }
   }
   
   private Vector3 RestrictPosition(Vector3 position)
   {
      // Restrict position to allowed boundaries
      position.y = Mathf.Max(position.y, minSpawnHeight);
      
      if (maxSpawnHeight > minSpawnHeight)
         position.y = Mathf.Min(position.y, maxSpawnHeight);
         
      position.x = Mathf.Clamp(position.x, minSpawnX, maxSpawnX);
      
      return position;
   }
    
   private bool IsPointerOverUI()
   {
      return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
   }
    
   private Vector3 GetMouseWorldPosition()
   {
      Vector3 mousePos = Input.mousePosition;
      mousePos.z = 10; 
      return mainCamera.ScreenToWorldPoint(mousePos);
   }
    
   private void FireProjectile(Vector3 position)
   {
      if (projectilePrefab != null)
      {
         Instantiate(projectilePrefab, position, Quaternion.identity);
      }
      else
      {
         Debug.LogError("Projectile prefab not assigned to Launcher");
      }
   }
    
   public void SetProjectile(GameObject newProjectilePrefab)
   {
      projectilePrefab = newProjectilePrefab;
   }
   
   private void OnDrawGizmos()
   {
      if (visualizeSpawnArea)
      {
         Gizmos.color = Color.yellow;
         Vector3 center = new Vector3((minSpawnX + maxSpawnX) * 0.5f, 
                                     (minSpawnHeight + (maxSpawnHeight > minSpawnHeight ? maxSpawnHeight : minSpawnHeight)) * 0.5f, 
                                     0);
                                     
         Vector3 size = new Vector3(maxSpawnX - minSpawnX, 
                                   (maxSpawnHeight > minSpawnHeight ? maxSpawnHeight - minSpawnHeight : 1), 
                                   0.1f);
                                   
         Gizmos.DrawWireCube(center, size);
      }
   }
}