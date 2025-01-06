using GameplayMechanics.Character;
using Player;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Handles the functionality for the enemy weapons
    /// </summary>
    public class EnemyWeapon : MonoBehaviour
    {
        public Collider collider;
        // Start is called before the first frame update
        void Awake()
        {
            collider = GetComponent<Collider>();
            collider.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            AbstractEnemy enemy = gameObject.GetComponentInParent<AbstractEnemy>();
            
            if ((PlayerStatManager.Instance != null && PlayerStatManager.Instance.IsBlocking && GameObject.FindGameObjectWithTag("Shield") != null && other.gameObject.tag == "Shield" && enemy.isAttackComplete)
                || (PlayerStatManager.Instance != null && PlayerStatManager.Instance.IsBlocking && GameObject.FindGameObjectWithTag("Weapon") != null && other.gameObject.tag == "Weapon" && enemy.isAttackComplete))
            {
                enemy.playerStats.TakeDamage(enemy.stats.Damage.GetAppliedTotal());
                if (transform.GetComponent<AudioSource>() != null)
                {
                    transform.GetComponent<AudioSource>().spatialBlend = 0f;
                    transform.GetComponent<AudioSource>().loop = false;
                    transform.GetComponent<AudioSource>().clip = Resources.Load("Audio/PlayerBlock") as AudioClip;
                    transform.GetComponent<AudioSource>().Play();
                }
            }
            
            if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon")) && enemy.isAttackComplete)
            {
                enemy.playerStats.TakeDamage(enemy.stats.Damage.GetAppliedTotal());
                if (transform.GetComponent<AudioSource>() != null)
                {
                    transform.GetComponent<AudioSource>().spatialBlend = 0f;
                    transform.GetComponent<AudioSource>().loop = false;
                    transform.GetComponent<AudioSource>().clip = Resources.Load("Audio/EnemyAttack") as AudioClip;
                    transform.GetComponent<AudioSource>().Play();
                }
            }
            gameObject.GetComponentInParent<AbstractEnemy>().isAttackComplete = false;
            collider.enabled = false;
            if (GameObject.FindGameObjectWithTag("Shield") != null)
            {
                GameObject.FindGameObjectWithTag("Shield").GetComponent<Collider>().enabled = true;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {

        }
    }
}
