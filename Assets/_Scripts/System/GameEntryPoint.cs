using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    private Canvas _loadingCanvas;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        _instance = new GameEntryPoint();
        _ = _instance.RunGame();
    }

    private GameEntryPoint()
    {
        CreateLoadingCanvas();
    }
    
    private void CreateLoadingCanvas()
    {
        var loadingPrefab = Resources.Load<Canvas>("LoadingCanvas");
        if (loadingPrefab != null)
        {
            _loadingCanvas = Object.Instantiate(loadingPrefab);
            Object.DontDestroyOnLoad(_loadingCanvas.gameObject);
            _loadingCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("LoadingCanvas prefab not found in Resources folder!");
        }
    }

    private async Task RunGame()
    {
        await LoadSceneAsync(Scenes.MAIN_MENU);
    }

    public static async void LoadMainMenu()
    {
        await LoadSceneAsync(Scenes.MAIN_MENU);
    }
    
    public static async void LoadGameplay()
    {
        await LoadSceneAsync(Scenes.GAMEPLAY);
    }

    private static async Task LoadSceneAsync(string targetScene, bool skipBoot = false)
    {
        Time.timeScale = 1f;
        ShowLoading();
        
        await Task.Delay(1000); //визуальная задержка
        
        if (!skipBoot)
        {
            var bootOperation = SceneManager.LoadSceneAsync(Scenes.BOOT);
            while (!bootOperation.isDone) 
                await Task.Yield();
        }
            
        var targetOperation = SceneManager.LoadSceneAsync(targetScene);
        while (!targetOperation.isDone) 
            await Task.Yield();
        
        await Task.Delay(500); //еще одна искусственная задержка
            
        HideLoading();
    }
    
    private static void ShowLoading()
    {
        if (_instance?._loadingCanvas != null)
            _instance._loadingCanvas.gameObject.SetActive(true);
        else
            Debug.LogWarning("Loading canvas not available!");
    }
    
    private static void HideLoading()
    {
        if (_instance?._loadingCanvas != null)
            _instance._loadingCanvas.gameObject.SetActive(false);
        else
            Debug.LogWarning("Loading canvas not available!");
    }
}