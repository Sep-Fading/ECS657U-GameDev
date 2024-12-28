using GameplayMechanics.Character;
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
        
        GameObject shopUI;

        private void Awake()
        {
            _anim = npcObject.GetComponent<Animator>();
            if (hasShop){
                shopUI = GameObject.Find("-- ShopBox");
            }
        }

        private void Start()
        {
            if (hasShop)
            {
                shopUI.GetComponent<ShopUI>().SetNpcShop(new NpcShop());
                shopUI.SetActive(false);
            }
        }

        public override void Interact()
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _anim.SetTrigger(Talking);
                if (!GameObject.Find("--DialogueBox").transform.GetChild(0).gameObject.active)
                {
                    GameObject.Find("--DialogueBox").transform.GetChild(0).gameObject.SetActive(true);
                    npcDialogue = GameObject.Find("--DialogueBox").transform.GetChild(0).GetComponent<NpcDialogue>();
                }
                if (hasShop){
                    npcDialogue.startDialogue(lines, npcName, hasQuest, hasShop, shopUI);
                }
                else
                {
                    npcDialogue.startDialogue(lines, npcName, hasQuest, hasShop);
                }
            }
        }
    }
}

