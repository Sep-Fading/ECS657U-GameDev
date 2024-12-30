using System;
using System.Threading;
using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;

namespace Weapons
{
    public class ThrowableAxe : MonoBehaviour
    {

        [SerializeField] Transform cam;
        [SerializeField] Transform throwPoint;
        [SerializeField] GameObject throwPrefab;
        [SerializeField] GameObject weaponSlot;
        [SerializeField] GameObject displayAxe;
        public float throwForce;
        public float throwUpwardForce;

        public bool readyToThrow;
        
        public GameObject currentAxe;

        private void Start()
        {
            readyToThrow = true;
        }

        private void OnEnable()
        {
            readyToThrow = true;
        }

        public void Throw()
        {
            if (Inventory.Instance.EquippedMainHand != null &&
                Inventory.Instance.EquippedMainHand.GetEquipmentType() == EquipmentType.AXE && currentAxe == null)
            {
                readyToThrow = true;
            }

            if (readyToThrow)
            {
                readyToThrow = false;

                GameObject throwable = Instantiate(throwPrefab, throwPoint.position, cam.rotation);
            

                Rigidbody rb = throwable.GetComponent<Rigidbody>();
            

                Vector3 forceDirection = cam.transform.forward;
            
                Vector3 forceToAdd = forceDirection * throwForce + cam.transform.up * throwUpwardForce;

                rb.AddForce(forceToAdd, ForceMode.Impulse);
                rb.angularVelocity = new Vector3(0f, 45f, 0f);

                currentAxe = throwable;

                if (weaponSlot.transform.childCount > 0)
                {
                    Destroy(weaponSlot.transform.GetChild(0).gameObject);
                }
                Invoke(nameof(ResetThrow), 0.5f);
            }
        }

        private void ResetThrow()
        {
            if (Inventory.Instance.EquippedMainHand != null && currentAxe != null)
            {
                currentAxe.GetComponent<AxeReturn>().ToggleReturn();
            }
            
        }
        
        public void DestroyAxe()
        {
            Destroy(currentAxe);
            currentAxe = null;
            Instantiate(displayAxe, weaponSlot.transform);
            readyToThrow = true;
        }

    }
}