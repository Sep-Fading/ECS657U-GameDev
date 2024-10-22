using GameplayMechanics.Character;
using UnityEngine;

namespace Combat
{
    // This script handles some basic states for Combat
    // And reflects them in the Animator.
    public class Weaponmanager : MonoBehaviour

    {
        private static readonly int Blocking = Animator.StringToHash("Blocking");
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private Animator _anim;
        private MeleeWeapon _currentWeapon;

        [SerializeField] private GameObject weaponPrefab;

        [SerializeField] private GameObject weaponColliderParent;
        private Collider _weaponCollider;

        private Shield _currentShield;
        private bool _blocking = false;

        private void Awake()
        {
            _anim = weaponPrefab.GetComponent<Animator>();
            if (weaponColliderParent != null)
            {
                _weaponCollider = weaponColliderParent.GetComponent<Collider>();
            }
        }

        public void Attack()
        {
            _anim.SetTrigger(Attacking);
        }

        public void Block()
        {
            _blocking = !_blocking;
            _anim.SetBool(Blocking, _blocking);
            PlayerStatManager.Instance.IsBlocking = _blocking;
        }

        public void OnBlockCancelled()
        {
            _blocking = false;
            _anim.SetBool(Blocking, _blocking);
            PlayerStatManager.Instance.IsBlocking = _blocking;
        }


        public void ChangeWeapon(MeleeWeapon newWeapon)
        {

        }
        public void ChangeShield(Shield newShield)
        {

        }

        public bool GetBlockingStatus() => _blocking;


    }
}
