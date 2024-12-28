using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadFire : MonoBehaviour
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
        if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("ShieldSlot")))
        {
            GameObject fireAOE = Instantiate(Resources.Load("FireAOE") as GameObject);
            fireAOE.transform.position = transform.position;
            if (transform.position.y <= 0.1f) fireAOE.transform.position = new Vector3(fireAOE.transform.position.x, 0.3f, fireAOE.transform.position.z);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerStatManager.Instance != null)
            {
                PlayerStatManager.Instance.TakeDamage(gameObject.GetComponentInParent<AbstractEnemy>().stats.Damage.GetCurrent());
            }
            }

        Destroy(gameObject);
    }
}
