using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager
{
    /// <summary>
    /// handles how the game state it saved between scenes
    /// </summary>
    public static GameStateManager Instance;
    private static bool _inSceneTransition = false;
        
    public static void Initialize()
    {
        if (Instance == null)
        {
            Instance = new GameStateManager();
        }
    }
        
    public void MoveToNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SetTransitionState(false);
    }
        
    public bool GetTransitionState() => _inSceneTransition;
    public bool SetTransitionState(bool state) => _inSceneTransition = state;
}