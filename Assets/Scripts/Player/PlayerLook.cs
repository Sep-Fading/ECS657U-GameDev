using UnityEngine;

namespace Player
{
    // A script handling the look-around functionality
    // for player locomotions.
    public class PlayerLook : MonoBehaviour
    {
        // Start is called before the first frame update
        public Camera cam;
        private float xRotation = 0f;
        public float xSensitivity = 30f;
        public float ySensitivity = 30f;
        public void ProcessLook(Vector2 input)
        {
            float mouseX = input.x;
            float mouseY = input.y;
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
