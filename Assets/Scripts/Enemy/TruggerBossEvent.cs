using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruggerBossEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Boss").GetComponent<AbstractEnemy>().SetState(EnemyState.TRIGGERED);
            Destroy(gameObject);
        }
    }
}
