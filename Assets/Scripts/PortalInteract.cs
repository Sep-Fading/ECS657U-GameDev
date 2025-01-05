using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteract : Interactable
{
    string[] scenes = { "Scenes/TestScene", "Scenes/Tutorial", "Scenes/World-v0.1", "Scenes/World-v0.2", "Scenes/World-v0.3", "Scenes/World-v0.4", "Scenes/World-v0.4_2" };

    public GameObject loadingCanvas; // Reference to the loading screen Canvas

    void Start()
    {
        HoverMessage = "Are You Ready to Move On [F]";
        if (loadingCanvas != null)
            loadingCanvas.SetActive(false); // Ensure the loading screen starts hidden
    }

    public override void Interact()
    {
        string nextScene = null;

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                nextScene = scenes[5];
                break;
            case 1:
                nextScene = scenes[3];
                break;
            case 2:
                nextScene = scenes[4];
                break;
            case 3:
                nextScene = scenes[5];
                break;
        }

        if (nextScene != null)
        {
            StartCoroutine(LoadSceneWithLoadingScreen(nextScene));
        }
    }

    private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        // Activate the loading screen
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true); // Show the loading screen
        }

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating immediately
        asyncOperation.allowSceneActivation = false;

        // Wait until the scene is ready to activate
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true; // Activate the scene
            }

            yield return null;
        }

        // Deactivate the loading screen after loading
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }
    }
}
