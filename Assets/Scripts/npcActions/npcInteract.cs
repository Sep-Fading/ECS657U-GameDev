using UnityEngine;

    public class npcInteract : Interactable
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
                npcDialogue.startDialogue( lines, npcName);
            }
        }
    }

