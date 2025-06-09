using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameLoader.LoadGameplay();
    }

    public void ToMainMenu()
    {
        GameLoader.LoadMainMenu();
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
