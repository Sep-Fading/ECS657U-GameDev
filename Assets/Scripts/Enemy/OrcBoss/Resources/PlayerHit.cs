using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    float thrownTime;
    // Start is called before the first frame update
    void Start()
    {
        thrownTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (thrownTime >= 1f) Destroy(gameObject);
        else thrownTime += Time.deltaTime;

        if (PlayerStatManager.Instance != null)
            if (!PlayerStatManager.Instance.IsBlocking) GameObject.FindGameObjectWithTag("ShieldSlot").GetComponentInChildren<Collider>().enabled = false;
            else GameObject.FindGameObjectWithTag("ShieldSlot").GetComponentInChildren<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStatManager.Instance.TakeDamage(gameObject.GetComponentInParent<AbstractEnemy>().stats.Damage.GetCurrent());
            Destroy(gameObject);
        }
    }
}
