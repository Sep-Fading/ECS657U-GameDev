using System.Collections;
using System.Collections.Generic;
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
    DummyScript enemy = target.GetComponent<DummyScript>();

    if (enemy != null) {
        enemy.SetHealth(enemy.GetHealth() - 10f);
    }

}
}
