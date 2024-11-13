using System;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class NpcDialogue : MonoBehaviour
    {
        public TextMeshProUGUI textComponent;

        public float textSpeed;
        private String[] lines;
        private String npcName;
        private int index;
        [SerializeField] private GameObject OptionContainer;
        [SerializeField] private Button button1;
        [SerializeField] private Button button2;
        [SerializeField] private Button NextButton;
    

        // Update is called once per frame
        private void Awake()
        {
            gameObject.SetActive(false);
            OptionContainer.SetActive(false);
            NextButton.onClick.AddListener(() => NextButtonOnClick());
        }
        public void startDialogue(String[] lines, String npcName)
        {
            gameObject.SetActive(true);
            UIManager.Instance.PushUI(gameObject);
        
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
                UIManager.Instance.PopUIByGameObject(gameObject);

                if (npcName == "Bernard")
                {
                    GameStateManager.Instance.SetTransitionState(true);
                }
            }
        }

        void NextButtonOnClick()
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
}
