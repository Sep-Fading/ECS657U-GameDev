using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public GameObject speechBubble; // Reference to the speech bubble GameObject
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
        if (playerIsNearby)
        {
            // Face the player for better visibility (optional)
            Vector3 lookDirection = (player.position - transform.position).normalized;
            lookDirection.y = 0; // Keep the speech bubble horizontal
            transform.forward = lookDirection;
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
