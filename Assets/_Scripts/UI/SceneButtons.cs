using UnityEngine;

public class SceneButtons : MonoBehaviour
{
    public void LoadMainMenu()
    {
        GameEntryPoint.LoadMainMenu();
    }
    
    public void LoadGameplay()
    {
        GameEntryPoint.LoadGameplay();
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}