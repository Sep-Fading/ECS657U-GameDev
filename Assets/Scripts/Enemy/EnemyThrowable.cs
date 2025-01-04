using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowable : MonoBehaviour
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
        if (thrownTime >= 1.5f) Destroy(gameObject);
        else thrownTime += Time.deltaTime;

        if (PlayerStatManager.Instance != null)
        {
            if (!PlayerStatManager.Instance.IsBlocking && GameObject.FindWithTag("Shield") != null) GameObject.FindWithTag("Shield").GetComponent<Collider>().enabled = false;
            else if (GameObject.FindWithTag("Shield") != null) GameObject.FindWithTag("Shield").GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        AbstractEnemy enemy = gameObject.GetComponentInParent<AbstractEnemy>();
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon"))
        {
            if (enemy != null)
            {
                PlayerStatManager.Instance.TakeDamage(enemy.stats.Damage.GetCurrent());
                enemy.audioSource.spatialBlend = 0f;
                enemy.audioSource.loop = false;
                enemy.audioSource.clip = Resources.Load("Audio/EnemyAttack") as AudioClip;
                enemy.audioSource.Play();
            }
            Destroy(gameObject);
        }
        else if ((PlayerStatManager.Instance != null && PlayerStatManager.Instance.IsBlocking && GameObject.FindGameObjectWithTag("Shield") != null && other.gameObject.tag == "Shield" && enemy.isAttackComplete)
            || (PlayerStatManager.Instance != null && PlayerStatManager.Instance.IsBlocking && GameObject.FindGameObjectWithTag("Weapon") != null && other.gameObject.tag == "Weapon" && enemy.isAttackComplete))
        {
            enemy.playerStats.TakeDamage(enemy.stats.Damage.GetAppliedTotal());
            if (enemy.audioSource != null)
            {
                enemy.audioSource.spatialBlend = 0f;
                enemy.audioSource.loop = false;
                enemy.audioSource.clip = Resources.Load("Audio/PlayerBlock") as AudioClip;
                enemy.audioSource.Play();
            }
            Destroy(gameObject);
        }
    }
}