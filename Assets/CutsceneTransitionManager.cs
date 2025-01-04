using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTransitionManager : MonoBehaviour
{
    // Called when cutscene is finished to transition to the next scene
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
