using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBossEvent : MonoBehaviour
{
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
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;
                GameObject.FindWithTag("Player").transform.position = new Vector3(-150f, 5f, 4017f);
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
            }
            if (GameObject.FindWithTag("Boss") != null) GameObject.FindWithTag("Boss").GetComponent<AbstractEnemy>().SetState(EnemyState.TRIGGERED);
        }
    }
}
