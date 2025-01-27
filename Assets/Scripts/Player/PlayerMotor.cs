using GameplayMechanics.Character;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    /// <summary>
    /// Script handling player locomotions &
    /// Movement related logic
    /// </summary>
    public class PlayerMotor : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;
        public float speed;
        public float normalSpeed = 5f;
        public float sprintSpeed = 8f;
        private float crouchSpeed = 3f;
        private float crouchRate = 2.5f; //2.5f is currently the most realistic
        public float gravity = -30f; //-30 is currently most realistic
        public float jumpheight = 1f;
        private bool onFloor;
        private bool LerpCrouch;
        private float crouchTimer;
        private bool Sprinting;
        private bool crouching;

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>(); 
            GameStateManager.Initialize();
            normalSpeed = PlayerStatManager.Instance.MoveSpeed.GetAppliedTotal();
            sprintSpeed = PlayerStatManager.Instance.SprintSpeed.GetAppliedTotal();
            speed = normalSpeed;
        }

        /*void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Motor");
            transform.position = new Vector3(0,0,0);
        }*/

        // Update is called once per frame
        void FixedUpdate()
        {
            //CROUCHING FUNCTIONALITY SECTION
            onFloor = controller.isGrounded;

            if (LerpCrouch)
            {
                crouchTimer += Time.deltaTime;
                speed = crouchSpeed;
                float p = crouchTimer * crouchRate;


                p *= p;
                if (crouching)
                {
                    controller.height = Mathf.Lerp(controller.height, 1, p);
                    Sprinting = false;
                }
                else
                {
                    controller.height = Mathf.Lerp(controller.height, 2, p);
                }

                if (p > 1)
                {
                    LerpCrouch = false;
                    crouchTimer = 0f;
                    if (crouching)
                    {
                        speed = crouchSpeed;
                    }
                    else
                    {
                        speed = normalSpeed;
                    }
                
                }
            }

        
        }

        public void ProcessMove(Vector2 input)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
            playerVelocity.y += gravity * Time.deltaTime;
            if (onFloor && playerVelocity.y < 0)
            {
                playerVelocity.y = -1f;
            }
            controller.Move(playerVelocity * Time.deltaTime);
        }


        public void Crouch(InputAction.CallbackContext context)
        {
            crouching = !crouching;
            crouchTimer = 0;
            LerpCrouch = true;
        }

        public bool getCrouching() 
        { 
            return crouching;
        }

        public void Sprint(InputAction.CallbackContext context)
        {

            if (!crouching && onFloor)
            {
                if (context.performed)
                {
                    Sprinting = true;
                }
                else
                {
                    Sprinting = false;
                }

                if (Sprinting)
                {
                    speed = sprintSpeed;
                }
                else
                {
                    speed = normalSpeed;
                }
            }
        
        }
        public void Jump(InputAction.CallbackContext context)
        {
            if (onFloor)
            {
                playerVelocity.y = Mathf.Sqrt(jumpheight * -2.0f * gravity);
            }
        }
    }
}
