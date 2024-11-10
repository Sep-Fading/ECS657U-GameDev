using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class NpcDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public float textSpeed;
    private String[] lines;
    private String npcName;

    private int index;

    // Update is called once per frame
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //only for testing bind to button later
        {
            if (textComponent.text == lines[index])
            {
                nextLine();   
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    
    }
    public void startDialogue(String[] lines, String npcName)
    {
        gameObject.SetActive(true);

        textComponent.text = String.Empty;
        this.lines = lines;
        this.npcName = npcName;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false); //REMOVE LATER WHEN ADDED BUTTONS TO INTERACT WITH
                                         //- THIS CLOSES THE UI WHEN DONE TALKING, BUT YOU MIGHT WANT TO STAY LONGER
        }
    }
}
