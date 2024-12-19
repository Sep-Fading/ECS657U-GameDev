// using UnityEngine;

// public class SpawnManager : MonoBehaviour
// {
//     public GameObject playerPrefab; // Player prefab
//     //public Transform spawnPoint;    // Reference to the spawn point in the scene

//     void Awake()
//     {
//         // Ensure spawnPoint is assigned; fallback to current transform if not set
//         if (spawnPoint == null)
//         {
//             spawnPoint = transform;
//         }
//     }

//     void Start()
//     {
//         // Check for existing player
//         GameObject player = GameObject.FindWithTag("Player");

//         // if (player != null)
//         // {
//         //     // Move existing player to spawn position and rotation
//         //     player.transform.position = spawnPoint.position;
//         //     player.transform.rotation = spawnPoint.rotation;
//         // }
//         // else
//         // {
//         //     // Instantiate the player prefab at the spawn point with correct rotation
//         //     Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
//         // }
//         player.transform.position = spawnPoint.position;
//         player.transform.rotation = spawnPoint.rotation;

//         Debug.Log("Player spawned at position: " + player.transform.position);
//         Debug.Log("Intended Spawn: " + spawnPoint.position);
//     }
// }
