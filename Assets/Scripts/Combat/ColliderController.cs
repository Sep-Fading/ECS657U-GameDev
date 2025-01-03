using UnityEngine;
using InventoryScripts;
namespace Combat
{
    public class ColliderController : MonoBehaviour
    {
        public void EnableCollider()
        {
            
            Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<BoxCollider>().enabled = true;
            
        }
        public void DisableCollider()
        {
            
            Inventory.Instance.EquippedMainHand.GetGameObject().GetComponent<BoxCollider>().enabled = false;
        }
    }
}