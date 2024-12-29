using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;
using Player;
using UI;

namespace Combat
{
    // This script handles some basic states for Combat
    // And reflects them in the Animator.
    public class Weaponmanager : MonoBehaviour

    {
        private static readonly int Blocking = Animator.StringToHash("Blocking");
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int ShieldAction = Animator.StringToHash("ShieldAction");
        private static readonly int LongSwordAction = Animator.StringToHash("LongSwordAction");
        private static readonly int MainHandAction = Animator.StringToHash("MainHandAction");
        
        
        
        
        private Animator _anim;
        private MeleeWeapon _currentWeapon;
        [SerializeField] private GameObject player;

        [SerializeField] private GameObject weaponPrefab;

        [SerializeField] private GameObject WeaponSlot;
        
        [SerializeField] private GameObject ShieldSlot;
        
        private Collider _weaponCollider;

        private Shield _currentShield;
        private bool _blocking = false;

        private void Awake()
        {
            _anim = weaponPrefab.GetComponent<Animator>();
        }

        public void Attack()
        { //add this line in the if statement after testing ----> && Inventory.Instance.EquippedMainHand != null
            if (UIManager.Instance.GetIsEmpty() )
            {
                _anim.SetTrigger(Attacking);
                
                if (WeaponSlot.transform.childCount > 0 && WeaponSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>() != null)
                {
                    
                    EquipmentInitializer target = WeaponSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>();
                    
                    if (target != null)
                    {
                        switch (target.GetEquipmentType())
                        {
                            case EquipmentType.GREATSWORD:
                                _anim.SetTrigger(LongSwordAction);
                                break;
                            case EquipmentType.MAINHAND:
                                _anim.SetTrigger(MainHandAction);
                                break;
                        }
                    }
                }
            }
        }

        public void Block()
        {
            if (UIManager.Instance.GetIsEmpty())
            {
                _blocking = !_blocking;
                _anim.SetBool(Blocking, _blocking);
                
                if (ShieldSlot.transform.childCount > 0 && ShieldSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>() != null)
                {
                    EquipmentInitializer target = ShieldSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>();
                    switch(target.GetEquipmentType())
                    {
                        case EquipmentType.SHIELD:
                            _anim.SetTrigger(ShieldAction);
                            break;
                        case EquipmentType.GREATSWORD:
                            _anim.SetTrigger(LongSwordAction);
                            break;
                    }
                }
                
                else
                {
                    if (WeaponSlot.transform.childCount > 0 && WeaponSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>() != null)
                    {
                        if (WeaponSlot.transform.GetChild(0).GetComponent<EquipmentInitializer>().GetEquipmentType() == EquipmentType.GREATSWORD)
                        {
                            _anim.SetTrigger(LongSwordAction);
                        }
                    }
                }
                PlayerStatManager.Instance.IsBlocking = _blocking;
            }
           
        }

        public void OnBlockCancelled()
        {
            _blocking = false;
            _anim.SetBool(Blocking, _blocking);
            PlayerStatManager.Instance.IsBlocking = _blocking;
        }

        public bool GetBlockingStatus() => _blocking;


    }
}
