using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneTransitionManager : MonoBehaviour
{
    // Called when cutscene is finished to transition to the next scene
    [SerializeField] private Sprite img1;
    [SerializeField] private Sprite img2;
    [SerializeField] private Sprite img3;
    [SerializeField] private GameObject imageContainer;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private string[] textarray;
    private int textindex = 0;
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void SwapToImage1()
    {
        imageContainer.GetComponent<Image>().sprite = img1;
    }
    public void SwapToImage2()
    {
        imageContainer.GetComponent<Image>().sprite = img2;
    }
    
    public void SwapToImage3()
    {
        imageContainer.GetComponent<Image>().sprite = img3;
    }

    public void StartText()
    {
        textbox.text = textarray[0];
    }
    
    public void NextText()
    {
        if (textindex < textarray.Length - 1)
        {
            textindex++;
            textbox.text = textarray[textindex];
        }
    }
}
