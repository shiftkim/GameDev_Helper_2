using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private GameObject pauseMenu;
    
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shop.gameObject.SetActive(true);
        }
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        pauseMenu.SetActive(true);
    }
   
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    
    private void Awake()
    {
        if(shop == null)
            Debug.LogError("UIManager: Shop is null");
        if(pauseMenu == null)
            Debug.LogError("UIManager: PauseMenu is null");
    }
   
    private void OnEnable()
    {
        Application.focusChanged += OnApplicationFocus;
    }
   
    private void OnDisable()
    {
        Application.focusChanged -= OnApplicationFocus;
    }
   
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !isPaused)
            PauseGame();
    }
}