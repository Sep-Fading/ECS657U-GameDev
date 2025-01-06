using UnityEngine;
using InventoryScripts;



namespace Combat
{
    ///<summary>
    /// This script handles when the colliders are activated during the animations of the weapons
    ///</summary>
    public class ColliderController : MonoBehaviour
    {
        public void EnableCollider()
        {
            var mainHand = GameObject.FindGameObjectWithTag("Weapon");
            if (mainHand != null)
            {
                var collider = mainHand.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
                else
                {
                    Debug.LogWarning("BoxCollider component not found on the equipped main hand.");
                }
            }
            else
            {
                Debug.LogWarning("EquippedMainHand is null.");
            }
        }

        public void DisableCollider()
        {
            var mainHand = GameObject.FindGameObjectWithTag("Weapon");
            if (mainHand != null)
            {
                var collider = mainHand.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
                else
                {
                    Debug.LogWarning("BoxCollider component not found on the equipped main hand.");
                }
            }
            else
            {
                Debug.LogWarning("EquippedMainHand is null.");
            }
        }
    }
}