using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Script used for triggering the Boss Events
/// </summary>
public class TriggerBossEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "World-v0.3")
            {
                Debug.LogWarning("TRIGGER");
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;
                GameObject.FindWithTag("Player").transform.position = new Vector3(286f, 5f, 430f);
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
            }
            if (SceneManager.GetActiveScene().name == "World-v0.4")
            {
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;
                GameObject.FindWithTag("Player").transform.position = new Vector3(-330f, 5f, 7795f);
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
            }
            if (SceneManager.GetActiveScene().name != "World-v0.4" && GameObject.FindWithTag("Boss") != null) { GameObject.FindWithTag("Boss").GetComponent<AbstractEnemy>().SetState(EnemyState.TRIGGERED); }
            if (GameObject.FindWithTag("Boss") != null && GameObject.FindWithTag("Boss").GetComponentInChildren<Canvas>()) { GameObject.FindWithTag("Boss").GetComponentInChildren<Canvas>().enabled = true; }

            if (GameObject.Find("-- Enemy") != null && GameObject.Find("-- Enemy").GetComponent<AudioSource>() != null && GetComponent<AudioSource>() != null)
            {
                GameObject.Find("-- Enemy").GetComponent<AudioSource>().Pause();
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
