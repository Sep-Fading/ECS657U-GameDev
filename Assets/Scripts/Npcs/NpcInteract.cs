using System;
using GameplayMechanics.Character;
using InventoryScripts;
using Npcs;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dialogue
{
    public class NpcInteract : Interactable
    {
        private Animator _anim;
        [SerializeField] private GameObject npcObject;
        private static readonly int Talking = Animator.StringToHash("talking");
        [SerializeField] private string[] lines;
        [SerializeField] private string npcName;
        [SerializeField] private NpcDialogue npcDialogue;
        public bool hasQuest;
        public bool hasShop;
        
        private void Awake()
        {
            _anim = npcObject.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        /*private void Start()
        {
            if (hasShop && shopUI == null)
            {
                shopUI = GameObject.FindGameObjectWithTag("ShopUI");
                shopUI.GetComponent<ShopUI>().SetNpcShop(new NpcShop());
                shopUI.SetActive(false);
            }
        }*/

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (hasShop)
            {
                GameObject shopUI = ShopManager.Instance.GetShopUI();
                if (shopUI != null)
                {
                    shopUI.SetActive(false);
                }
            }
        }

        public override void Interact()
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _anim.SetTrigger(Talking);

                GameObject dialogueBox = GameObject.Find("--DialogueBox").transform.GetChild(0).gameObject;

                if (!dialogueBox.activeSelf)
                {
                    dialogueBox.SetActive(true);
                    npcDialogue = dialogueBox.GetComponent<NpcDialogue>();
                }

                if (hasShop)
                {
                    npcDialogue.startDialogue(lines, npcName, hasQuest, hasShop, ShopManager.Instance.GetShopUI());
                }
                else
                {
                    npcDialogue.startDialogue(lines, npcName, hasQuest, hasShop);
                }
            }
        }

    }
}

