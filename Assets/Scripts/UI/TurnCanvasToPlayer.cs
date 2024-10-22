using UnityEngine;

namespace UI
{
    public class TurnCanvasToPlayer : MonoBehaviour
    {
        Camera _playerCamera;

        void Start()
        {
            _playerCamera = Camera.main;
        }
        void Update()
        {
            transform.LookAt(transform.position + _playerCamera.transform.rotation *
                Vector3.forward, _playerCamera.transform.rotation * Vector3.up);
        }
    }
}
    