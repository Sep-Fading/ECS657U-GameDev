using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public GameObject speechBubble; // Reference to the speech bubble container
    public float displayTime = 3f; // Time the bubble stays visible after appearing

    private Transform player; // Reference to the player's transform
    private bool playerIsNearby = false;

    void Start()
    {
        // Ensure the speech bubble starts hidden
        if (speechBubble != null)
            speechBubble.SetActive(false);

        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerIsNearby && speechBubble != null)
        {
            // Rotate the speech bubble to face the player
            Vector3 lookDirection = (player.position - speechBubble.transform.position).normalized;
            lookDirection.y = 0; // Keep the bubble's rotation horizontal
            speechBubble.transform.forward = lookDirection;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;

            // Show the speech bubble
            if (speechBubble != null)
                speechBubble.SetActive(true);

            // Optionally hide the speech bubble after a delay
            Invoke(nameof(HideSpeechBubble), displayTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;

            // Hide the speech bubble
            if (speechBubble != null)
                speechBubble.SetActive(false);
        }
    }

    void HideSpeechBubble()
    {
        if (speechBubble != null)
            speechBubble.SetActive(false);
    }
}
