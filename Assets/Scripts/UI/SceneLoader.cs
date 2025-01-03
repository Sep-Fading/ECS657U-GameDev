using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Canvas loadingCanvas; // Reference to the loading screen Canvas
    public Slider progressBar; // Optional: Progress bar Slider

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show the loading screen
        loadingCanvas.gameObject.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating immediately
        asyncOperation.allowSceneActivation = false;

        // Update progress
        while (!asyncOperation.isDone)
        {
            // Update the progress bar (0 to 0.9 is the progress range)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (progressBar != null)
            {
                progressBar.value = progress; // Update Slider value
            }

            // Check if the scene is ready to be activated
            if (asyncOperation.progress >= 0.9f)
            {
                // Optionally display a "Press Any Key" message here

                // Activate the scene
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Hide the loading screen after loading
        loadingCanvas.gameObject.SetActive(false);
    }
}
