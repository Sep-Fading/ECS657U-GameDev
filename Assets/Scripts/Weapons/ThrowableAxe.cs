using System;
using System.Threading;
using UnityEngine;

namespace Weapons
{
    public class ThrowableAxe : MonoBehaviour
    {

        [SerializeField] Transform cam;
        [SerializeField] Transform ThrowPoint;
        [SerializeField] GameObject ThrowPrefab;
        [SerializeField] GameObject WeaponSlot;
        [SerializeField] GameObject DisplayAxe;
        public float throwForce;
        public float throwUpwardForce;

        public bool readyToThrow;
        
        public GameObject currentAxe;

        private void Start()
        {
            readyToThrow = true;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) && readyToThrow) // TODO must bind this through the actual keybinding scheme
            {
                Throw();
            }
        }

        private void Throw()
        {
            readyToThrow = false;

            GameObject throwable = Instantiate(ThrowPrefab, ThrowPoint.position, cam.rotation);
            

            Rigidbody rb = throwable.GetComponent<Rigidbody>();
            

            Vector3 forceDirection = cam.transform.forward;
            
            Vector3 forceToAdd = forceDirection * throwForce + cam.transform.up * throwUpwardForce;

            rb.AddForce(forceToAdd, ForceMode.Impulse);
            rb.angularVelocity = new Vector3(0f, 45f, 0f);

            currentAxe = throwable;

            Destroy(WeaponSlot.transform.GetChild(0).gameObject);
            Invoke(nameof(ResetThrow), 0.5f);
        }

        private void ResetThrow()
        {
            currentAxe.GetComponent<AxeReturn>().ToggleReturn();
        }
        
        public void DestroyAxe()
        {
            Destroy(currentAxe);
            currentAxe = null;
            Instantiate(DisplayAxe, WeaponSlot.transform);
            readyToThrow = true;
        }

    }
}