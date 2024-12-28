using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipTransform; // The RectTransform of the tooltip
    [SerializeField] private TextMeshProUGUI tooltipText; // The Text component for the tooltip content
    [SerializeField] private TextMeshProUGUI tooltipTitle; // The Text component for the tooltip title
    [SerializeField] private TextMeshProUGUI tooltipPrice; // The Text component for the tooltip price
    [SerializeField] private Vector2 offset = new Vector2(10f, 10f); // Offset from the mouse position

    void Start()
    {
        // Ensure the tooltip starts hidden
        HideTooltip();
    }

    void Update()
    {
        if (tooltipTransform.gameObject.activeSelf)
        {
            // Update tooltip position to follow the mouse
            Vector3 mousePosition = Input.mousePosition;

            // Adjust the tooltip's position with an offset
            tooltipTransform.position = mousePosition + (Vector3) offset;

            // Optional: Clamp tooltip within screen bounds
            ClampToScreenBounds();
        }
    }

    public void ShowTooltip(string title, string content, string price)
    {
        tooltipTitle.text = title;
        tooltipText.text = content;
        tooltipPrice.text = price;
        tooltipTransform.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipText.text = string.Empty;
        tooltipTransform.gameObject.SetActive(false);
    }

    private void ClampToScreenBounds()
    {
        Vector3[] corners = new Vector3[4];
        tooltipTransform.GetWorldCorners(corners);
        Vector3 position = tooltipTransform.position;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Adjust position to ensure the tooltip stays within the screen
        if (corners[2].x > screenWidth) position.x -= corners[2].x - screenWidth; // Right
        if (corners[0].x < 0) position.x += 0 - corners[0].x;                     // Left
        if (corners[2].y > screenHeight) position.y -= corners[2].y - screenHeight; // Top
        if (corners[0].y < 0) position.y += 0 - corners[0].y;                     // Bottom

        tooltipTransform.position = position;
    }
}