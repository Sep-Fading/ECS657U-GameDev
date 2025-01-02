using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapCanvas; // Reference to the Canvas containing the map

    void Update()
    {
        //Check if the 'M' key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Toggle the map's visibility
            mapCanvas.SetActive(!mapCanvas.activeSelf);
        }
    }
}
