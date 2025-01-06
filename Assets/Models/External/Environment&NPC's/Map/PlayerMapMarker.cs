using UnityEngine;

public class PlayerMapMarker : MonoBehaviour
{
    public RectTransform playerMarker; // The UI marker representing the player
    public RectTransform mapRectTransform; // The RectTransform of the map Image

    public Vector2 map2Size = new Vector2(500, 500); // Map 2: 500x500 terrain size
    public Vector2 map3Size = new Vector2(250, 250); // Map 3: 250x250 terrain size

    public Vector2 uiMapSize = new Vector2(500, 500); // Map Image size in pixels

    public Vector3 map2Origin = new Vector3(0, 0, 0); // Bottom-left corner of Map 2
    public Vector3 map3Origin = new Vector3(-125, 0, -125); // Bottom-left corner of Map 3

    private Vector2 activeMapSize; // Currently active map size in world units
    private Vector3 activeMapOrigin; // Currently active map origin in world units
    private Transform playerTransform; // The player's Transform, dynamically assigned

    void Start()
    {
        FindPlayer(); // Attempt to find the player dynamically
        SetMapConfiguration(3); // Default to Map 2 configuration
    }

    void Update()
    {
        if (playerTransform == null)
        {
            FindPlayer(); // Retry finding the player if it's missing
            return;
        }

        if (playerMarker == null || mapRectTransform == null)
        {
            Debug.LogWarning("Marker or Map RectTransform is not assigned!");
            return;
        }

        // Safeguard: Avoid division by zero
        if (activeMapSize.x == 0 || activeMapSize.y == 0 || uiMapSize.x == 0 || uiMapSize.y == 0)
        {
            Debug.LogWarning("Map size or UI map size cannot be zero.");
            return;
        }

        // Get the player's position in the world
        Vector3 playerPosition = playerTransform.position;

        // Adjust the player's position relative to the active map's origin
        Vector3 adjustedPosition = playerPosition - activeMapOrigin;

        // Convert the adjusted position to normalized map coordinates
        float normalizedX = Mathf.InverseLerp(0, activeMapSize.x, adjustedPosition.x);
        float normalizedY = Mathf.InverseLerp(0, activeMapSize.y, adjustedPosition.z);

        // Convert normalized coordinates to map UI coordinates
        float mapX = normalizedX * uiMapSize.x;
        float mapY = normalizedY * uiMapSize.y;

        // Update the marker's position on the map
        playerMarker.localPosition = new Vector3(mapX - uiMapSize.x / 2, mapY - uiMapSize.y / 2, 0);

        // Rotate the marker to match the player's facing direction
        float playerYaw = playerTransform.eulerAngles.y;
        playerMarker.localRotation = Quaternion.Euler(0, 0, -playerYaw);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            Debug.Log("Player found and assigned to the map marker.");
        }
        else
        {
            Debug.LogWarning("Player not found. Ensure it is tagged as 'Player'.");
        }
    }

    public void SetMapConfiguration(int mapIndex)
    {
        if (mapIndex == 2)
        {
            activeMapSize = map2Size; // Use Map 2 size
            activeMapOrigin = map2Origin; // Use Map 2 origin
        }
        else if (mapIndex == 3)
        {
            activeMapSize = map3Size; // Use Map 3 size
            activeMapOrigin = map3Origin; // Use Map 3 origin
        }
        else
        {
            Debug.LogWarning("Invalid map index provided!");
        }

        Debug.Log($"Switched to Map {mapIndex}: Active Map Size = {activeMapSize}, Origin = {activeMapOrigin}");
    }
}
