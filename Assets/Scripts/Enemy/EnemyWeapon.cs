using GameplayMechanics.Character;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyWeapon : MonoBehaviour
    {
        public Collider collider;
        // Start is called before the first frame update
        void Awake()
        {
            collider = GetComponent<Collider>();
            collider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void OnTriggerEnter(Collider other)
        {
            AbstractEnemy enemy = gameObject.GetComponentInParent<AbstractEnemy>();
            
            if ((PlayerStatManager.Instance != null && PlayerStatManager.Instance.IsBlocking && GameObject.FindGameObjectWithTag("Shield") != null && other.gameObject.tag == "Shield" && enemy.isAttackComplete)
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
            }
            
            if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon")) && enemy.isAttackComplete)
            {
                enemy.playerStats.TakeDamage(enemy.stats.Damage.GetAppliedTotal());
                if (enemy.audioSource != null)
                {
                    enemy.audioSource.spatialBlend = 0f;
                    enemy.audioSource.loop = false;
                    enemy.audioSource.clip = Resources.Load("Audio/EnemyAttack") as AudioClip;
                    enemy.audioSource.Play();
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
