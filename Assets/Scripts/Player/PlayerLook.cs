using UnityEngine;

namespace Player
{
    /// <summary>
    /// A script handling the look-around functionality
    /// for player locomotions.
    /// </summary>
    public class PlayerLook : MonoBehaviour
    {
        // Start is called before the first frame update
        public Camera cam;
        private float xRotation = 0f;
        private float xSensitivity = 10f;
        private float ySensitivity = 10f;
        public void ProcessLook(Vector2 input)
        {
            float mouseX = input.x;
            float mouseY = input.y;
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
        
        public void UpdateSensitivity(float sensitivity)
        {
            xSensitivity = sensitivity;
            ySensitivity = sensitivity;
        }
    }
}
