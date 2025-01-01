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
            //Debug.Log(gameObject.GetComponentInParent<AbstractEnemy>().isAttackComplete);
            // Check if the object collided with is the player
            AbstractEnemy enemy = gameObject.GetComponentInParent<AbstractEnemy>();
            if (other.gameObject.CompareTag("Player") && enemy.isAttackComplete)
            {
                // Perform damage or other logic
                Debug.Log("Player hit!");
                // Reset the attack state
                enemy.playerStats.TakeDamage(enemy.stats.Damage.GetAppliedTotal());
                Debug.Log("Player HP: " + enemy.playerStats.Life.GetCurrent() + "/" + enemy.playerStats.Life.GetFlat());
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
