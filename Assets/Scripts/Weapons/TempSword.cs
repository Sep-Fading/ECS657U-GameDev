using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;


public class TempSword : MonoBehaviour
{
    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision){
        GameObject target = collision.gameObject;

        if (target.tag == "Enemy" || target.tag == "Boss") 
        {
            AbstractEnemy enemy = target.GetComponent<AbstractEnemy>();
            if (enemy != null)
            {
                PlayerStatManager.Instance.DoDamage(enemy);
                Debug.Log($"{enemy.stats.Life.GetCurrent()}/{enemy.stats.Life.GetFlat()}");
            }
        }

    }
}
