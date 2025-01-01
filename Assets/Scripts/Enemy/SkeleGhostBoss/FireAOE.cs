using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAOE : MonoBehaviour
{
    float activeTime = 5f;
    float damageCountdown = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatManager.Instance != null)
        {
            if (!PlayerStatManager.Instance.IsBlocking)
            {
                if (GameObject.FindWithTag("Shield") != null)
                {
                    GameObject.FindWithTag("Shield").GetComponent<Collider>().enabled = false;
                }
            }
            else
            {
                if (GameObject.FindWithTag("Shield") != null)
                {
                    GameObject.FindWithTag("Shield").GetComponent<Collider>().enabled = true;
                }
            }
        }
        if (damageCountdown <= 0f)  
        {
            damageCountdown = 1f;
            foreach (Collider objects in Physics.OverlapSphere(transform.position, 3f))
            {
                if (objects.tag == "Player")
                {
                    PlayerStatManager.Instance.TakeDamage(20f); break;
                }
            }
        }
        else
        {
            damageCountdown -= Time.deltaTime;
        }
        if (activeTime <= 0f) Destroy(gameObject);
        else activeTime -= Time.deltaTime;
    }
}
