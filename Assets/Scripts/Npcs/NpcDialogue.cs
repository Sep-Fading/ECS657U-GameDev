using System;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Npcs
{
    /// <summary>
    /// Handles how the text appears when talking with an Npc
    /// </summary>
    public class NpcDialogue : MonoBehaviour
    {
        public TextMeshProUGUI textComponent;

        public float textSpeed;
        private String[] lines;
        private String npcName;
        private int index;
        [SerializeField] private GameObject OptionContainer;
        [SerializeField] private Button AcceptButton;
        [SerializeField] private Button DeclineButton;
        [SerializeField] private Button ShopButton;
        [SerializeField] private Button NextButton;

        GameObject shopUI;
        // Update is called once per frame
        private void Awake()
        {
            gameObject.SetActive(false);
            AcceptButton.gameObject.SetActive(false);
            DeclineButton.gameObject.SetActive(false);
            ShopButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
            NextButton.onClick.AddListener(() => NextButtonOnClick());
            ShopButton.onClick.AddListener(() => ShopButtonOnClick());
        }
        public void startDialogue(String[] lines, String npcName, bool hasQuest, bool hasShop, GameObject shopUI = null)
        {
            gameObject.SetActive(true);
            UIManager.Instance.PushUI(gameObject);
            this.shopUI = shopUI;

            textComponent.text = String.Empty;
            this.lines = lines;
            this.npcName = npcName;
            index = 0;

            // Set NPC options here with if/else statements.
            if (hasShop)
            {
                ShopButton.gameObject.SetActive(true);
            }
            else if (hasQuest)
            {
                NextButton.gameObject.SetActive(false);
                ShopButton.gameObject.SetActive(false);
                AcceptButton.gameObject.SetActive(true);
                DeclineButton.gameObject.SetActive(true);
                AcceptButton.onClick.AddListener(() => AcceptButtonOnClick());
                DeclineButton.onClick.AddListener(() => DeclineButtonOnClick());
            }
            else
            {
                NextButton.gameObject.SetActive(true);
                ShopButton.gameObject.SetActive(false);
                AcceptButton.gameObject.SetActive(false);
                DeclineButton.gameObject.SetActive(false);
            }

            StartCoroutine(TypeLine());
        }

        private void DeclineButtonOnClick()
        {
            throw new NotImplementedException();
        }

        private void AcceptButtonOnClick()
        {
            throw new NotImplementedException();
        }

        private void ShopButtonOnClick()
        {
            shopUI.GetComponent<ShopUI>().OpenShop();
            UIManager.Instance.PopUIByGameObject(gameObject);
        }

        IEnumerator TypeLine()
        {
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
                    if (GameObject.Find("Portal") != null)
                    {
                        if (GameObject.Find("Portal").GetComponent<AudioSource>() != null)
                        {
                            GameObject.Find("Portal").GetComponent<AudioSource>().Play();
                        }
                        GameObject.Find("Portal").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("PortalHole") != null)
                        {
                            GameObject.Find("PortalHole").GetComponent<Renderer>().enabled = true;
                        }
                    }
                }

                if (npcName == "Death")
                {
                    GameObject death = Instantiate(Resources.Load("Death"), GameObject.Find("Bernard").transform.position, transform.localRotation) as GameObject;
                    Destroy(GameObject.Find("Bernard"), 5f);
                }

                if (npcName == "Shopkeeper")
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