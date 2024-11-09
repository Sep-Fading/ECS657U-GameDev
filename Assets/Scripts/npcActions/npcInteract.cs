using UnityEngine;

    public class npcInteract : Interactable
    {
        private Animator _anim;
        [SerializeField] private GameObject npcObject;
        private static readonly int Talking = Animator.StringToHash("talking");
        
        private void Awake()
        {
            _anim = npcObject.GetComponent<Animator>();
        }
        public override void Interact()
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _anim.SetTrigger(Talking);
            }
       
        }
    }

