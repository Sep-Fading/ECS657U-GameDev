using GameplayMechanics.Character;
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

        private void Awake()
        {
            _anim = npcObject.GetComponent<Animator>();
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
                npcDialogue.startDialogue(lines, npcName);
            }
        }
    }
}

