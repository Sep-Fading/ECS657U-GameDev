using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
    public int targetSceneName = 2; // Scene to teleport to
    public TMP_Text promptText; // TextMeshPro Text for the prompt

    private bool playerIsNear = false;

    GameObject player;

    // Static spawn position to be used in the new scene
    public static Vector3 spawnPosition;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (promptText != null)
            promptText.enabled = false; // Hide text initially
    }

    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.T))
        {
            // Store the spawn position (e.g., at a spawn point's position)
            //spawnPosition = new Vector3(0, 2, 0); // Adjust Y for height on terrain
            if (targetSceneName == 2)
            {
                player.transform.position = new Vector3(441.6f, 2f, 230f);
            }
            TeleportPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            playerIsNear = true;
            if (promptText != null)
                promptText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (promptText != null)
                promptText.enabled = false;
        }
    }

    void TeleportPlayer()
    {
        Debug.Log("Player spawned at position: " + player.transform.position);
        Debug.Log("Intended Spawn: 441.6f, 2f, 230f");
        SceneManager.LoadScene(targetSceneName);
    }
}
