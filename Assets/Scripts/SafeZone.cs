using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public float retreatSpeed = 5f; // Speed at which enemies move away from the safe zone

    private void OnTriggerStay(Collider other)
    {
        // Get the parent GameObject (the main enemy object)
        GameObject enemy = other.gameObject;

        // Confirm the object has the "Enemy" tag
        if (enemy.CompareTag("Enemy"))
        {
            // Debug log to confirm detection
            Debug.Log($"{enemy.name} is inside the safe zone.");

            // Get the retreat direction (away from the safe zone)
            Vector3 retreatDirection = (enemy.transform.position - transform.position).normalized;

            // Ensure there's a valid direction
            if (retreatDirection.magnitude < 0.1f)
            {
                retreatDirection = new Vector3(1, 0, 0); // Default direction if too close
            }

            // Rotate the enemy to face away from the safe zone
            enemy.transform.rotation = Quaternion.LookRotation(retreatDirection);

            // Move the enemy in the retreat direction
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Use Rigidbody for smoother movement
                rb.velocity = retreatDirection * retreatSpeed;
            }
            else
            {
                // Use transform movement as a fallback
                enemy.transform.position += retreatDirection * retreatSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is an enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"{other.name} has exited the safe zone.");
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the safe zone in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
