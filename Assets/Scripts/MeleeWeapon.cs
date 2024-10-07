using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    private string weaponName;

    public float attackDistance;
    [SerializeField]
    public float attackSpeed;
    [SerializeField]
    public int attackDamage;
    [SerializeField]
    public LayerMask attackLayer;

    bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
