using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        private TextMeshProUGUI tooltipTextTitle;
        private TextMeshProUGUI tooltipTextDesc;
        private RectTransform backgroundRectTransform;
        [SerializeField] private float textPadding = 5f;
        private void Awake()
        {
            backgroundRectTransform = transform.Find("TooltipBackground").GetComponent<RectTransform>();
            tooltipTextTitle = transform.Find("TooltipText_Title").GetComponent<TextMeshProUGUI>();
            tooltipTextDesc = transform.Find("TooltipText_Description").GetComponent<TextMeshProUGUI>();
        }

        private void ShowTooltip(string title, string desc)
        {
            gameObject.SetActive(true);
            tooltipTextTitle.text = title;
            tooltipTextDesc.text = desc;
            
            // Tool tip size
            Vector2 backgroundSize = new Vector2(
                tooltipTextTitle.preferredWidth + tooltipTextDesc.preferredWidth + (textPadding*2f),
                tooltipTextTitle.preferredHeight + tooltipTextDesc.preferredHeight + (textPadding*2f));
            backgroundRectTransform.sizeDelta = backgroundSize;
        }

        private void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    
    }
}
