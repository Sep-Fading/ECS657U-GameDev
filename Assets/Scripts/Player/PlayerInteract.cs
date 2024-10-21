using InventoryScripts;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        private Camera _cam;
        [FormerlySerializedAs("InteractableDistance")] [SerializeField]
        private float interactableDistance = 3f;
        [SerializeField]
        private LayerMask mask;
        private PlayerUI _playerUI;
        private InputManager _inputManager;
        private PickUpManager _pickUpManager;

        public bool canPickUp;
        // Start is called before the first frame update
        void Start()
        {
            _cam = GetComponent<PlayerLook>().cam;
            _playerUI = GetComponent<PlayerUI>();
            _inputManager = GetComponent<InputManager>();
            _pickUpManager = GetComponent<PickUpManager>();
        }

        // Update is called once per frame
        void Update()
        {

            _playerUI.UpdateText(string.Empty);

            Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);

            Debug.DrawRay(ray.origin, ray.direction * interactableDistance);

            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, interactableDistance, mask))
            {
                if(hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactedObj = hitInfo.collider.GetComponent<Interactable>();
                    _playerUI.UpdateText(interactedObj.HoverMessage);
                    if(_inputManager.PlayerInput.grounded.Interacting.triggered) 
                    {
                        interactedObj.BaseInteract();
                    }
                }
            }
        
            // Pick up functionality:
            if (hitInfo.collider != null)
            {
                EquipmentInitializer component = hitInfo.collider.GetComponent<EquipmentInitializer>();
                if (component != null)
                {
                    _pickUpManager.SetItemToPickUp(component);
                    canPickUp = true;
                }
                else
                {
                    canPickUp = false;
                }
            }
            else
            {
                canPickUp = false;
            }
        }
    }
}
