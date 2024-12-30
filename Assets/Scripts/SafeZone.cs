using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public float safeZoneRadius = 10f; // Radius of the safe zone
    public float retreatSpeed = 5f; // Speed at which enemies move away from the safe zone

    private void OnTriggerStay(Collider other)
    {
        // Check if the object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Get the direction away from the safe zone
            Vector3 retreatDirection = (other.transform.position - transform.position).normalized;

            // Ensure the enemy has a minimum retreat vector
            if (retreatDirection.magnitude < 0.1f)
            {
                retreatDirection = new Vector3(1, 0, 0); // Default direction if too close
            }

            // Rotate the enemy to face away from the safe zone
            other.transform.rotation = Quaternion.LookRotation(retreatDirection);

            // Move the enemy in the retreat direction
            other.transform.position += retreatDirection * retreatSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Reset enemy behavior when it leaves the safe zone
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"{other.name} has exited the safe zone.");
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the safe zone radius in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeZoneRadius);
    }
}
