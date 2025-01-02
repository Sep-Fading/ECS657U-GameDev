using UnityEngine;
using InventoryScripts;
namespace Combat
{
    public class ColliderController : MonoBehaviour
    {
        public void EnableCollider()
        {
            if (Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<Collider>().enabled != true)
            {
                Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<Collider>().enabled = true;
            }
        }
        public void DisableCollider()
        {
            if (Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<Collider>().enabled)
            {
                Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<Collider>().enabled = false;
            }
        }
    }
}