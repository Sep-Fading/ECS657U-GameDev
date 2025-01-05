using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAreaNameCard : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float duration = 2f; //Fade out over 2 seconds.
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        textDisplay.enabled = false;
        yield break;
    }
}
