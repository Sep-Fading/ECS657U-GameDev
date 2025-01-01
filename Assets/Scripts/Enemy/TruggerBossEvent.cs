using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;
                GameObject.FindWithTag("Player").transform.position = new Vector3(286f, 2.2f, 427f);
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
            }
            GameObject.FindWithTag("Boss").GetComponent<AbstractEnemy>().SetState(EnemyState.TRIGGERED);
            Destroy(gameObject);
        }
    }
}
