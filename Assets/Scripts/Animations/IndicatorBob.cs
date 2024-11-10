using UnityEngine;

public class IndicatorBob : MonoBehaviour
{
    [SerializeField] private float bobbingHeight = 0.2f; // How high the text bobs up and down
    [SerializeField] private float bobbingSpeed = 2f; // How fast the text bobs up and down
    private Vector3 startPosition; // Original position of the text

    void Start()
    {
        // Store the starting position of the text
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the new position with a sine wave
        float newZ = startPosition.z + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.localPosition = new Vector3(startPosition.x, startPosition.y, newZ);
    }
}