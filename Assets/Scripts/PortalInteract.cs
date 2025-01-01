using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteract : Interactable
{
    string[] scenes = { "Scenes/TestScene","Scenes/Tutorial", "Scenes/World-v0.1", "Scenes/World-v0.2", "Scenes/World-v0.3", "Scenes/World-v0.4", "Scenes/World-v0.4_2"};
    // Start is called before the first frame update
    void Start()
    {
        HoverMessage = "Are You Ready to Move On [F]";
    }
    public override void Interact()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                GameStateManager.Instance.MoveToNextScene(scenes[5]);
                break;
            case 1:
                GameStateManager.Instance.MoveToNextScene(scenes[3]);
                break;
            case 2:
                GameStateManager.Instance.MoveToNextScene(scenes[4]);
                break;
            case 3:
                GameStateManager.Instance.MoveToNextScene(scenes[5]);
                break;
        }
    }
}
