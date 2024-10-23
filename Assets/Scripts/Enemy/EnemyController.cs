using System.Collections;
using GameplayMechanics.Character;
using Player;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        GameObject player;
        public Transform playerTransform; 
        public PlayerMotor playerMotor;
        public Collider wallCollider;
        public float speed;
        public float minDistance = 1.8f;
        public float maxDistance = 10f;
        float idleTime = 0f;
        float attackCooldown = 1f;
        public LayerMask obstacleLayer;
        float randomRot;
        private bool isRotating = false;
        private bool triggered;
        public float maxHealth = 100f;
        public float currentHealth;
        public Animator playerWeaponAnimator;
        public Material triggeredMaterial;
        public Material defaultMaterial;
        Renderer enemyRenderer;
        public TextMeshProUGUI healthText;
        private MaterialPropertyBlock propBlock;

        // Start is called before the first frame update
        void Start()
        {
            speed = 4f;
            triggered = false;
            //enemyRenderer = GetComponent<Renderer>();
            randomRot = Random.Range(-360f, 360f);
            currentHealth = maxHealth;
        }

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerMotor = player.GetComponent<PlayerMotor>();
            }
            playerWeaponAnimator = GameObject.FindWithTag("WeaponHolder").GetComponent<Animator>();
            enemyRenderer = GetComponent<Renderer>();
            enemyRenderer.material = defaultMaterial;
            wallCollider = GameObject.FindWithTag("Environment").GetComponent<Collider>();

            propBlock = new MaterialPropertyBlock();
        }

        // Update is called once per frame
        void Update()
        {
            if (currentHealth <= 0)
            {
                defeatEnemy();
            }

            Vector3 playerPos = playerTransform.position;
            float distance = Vector3.Distance(transform.position, playerPos);
            bool obstacleBetween = Physics.Raycast(transform.position, (playerPos - transform.position).normalized, distance, obstacleLayer);
            healthText.text = currentHealth + "/" + maxHealth;

            if (playerMotor.getCrouching() && !triggered)
            {
                maxDistance = minDistance = 0;
            }
            else
            {
                minDistance = 1.8f;
                maxDistance = 10f;
            }

            bool inChaseRange = distance > minDistance && distance <= maxDistance && !obstacleBetween;
            bool inAttackRange = distance <= minDistance;

            //enemyRenderer.material = triggered ? triggeredMaterial : defaultMaterial;
            //Color healthColor = enemyRenderer.material.color;
            //healthColor.a = (currentHealth/maxHealth);
            //enemyRenderer.material.color = healthColor;
            enemyRenderer.GetPropertyBlock(propBlock);
            Color healthColor = triggered ? triggeredMaterial.color : defaultMaterial.color;
            healthColor.a = currentHealth / maxHealth;
            propBlock.SetColor("_Color", healthColor);
            enemyRenderer.SetPropertyBlock(propBlock);

            if (triggered)
            {
                if (inChaseRange)
                {
                    trackPlayer(playerPos);
                }
                else if (inAttackRange)
                {
                    attack();
                }
            }
            else
            {
                idle();
            }

            triggered = inChaseRange || inAttackRange;
        }

        void idle()
        {
            triggered = false;
            idleTime += Time.deltaTime;
            speed = 1f;
            if (!isRotating)
            {
                StartCoroutine(SmoothTurn(Random.Range(-360f, 360f), 5f));
            }
            else if (idleTime > 2f)
            {
                idleTime = 0f;
            }
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        void trackPlayer(Vector3 playerPos) 
        {
            speed = 4.5f;
            isRotating = false;
            triggered = true;
            transform.LookAt(playerPos);
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        void attack() 
        { 
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                PlayerStatManager.Instance.TakeDamage(15f);
                attackCooldown = 1f;
            }
        }

        void defeatEnemy() 
        {
            XpManager.GiveXp(10f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Wall" && !isRotating)
            {
                StartCoroutine(SmoothTurn(Random.Range(90f, 180f), 1f));
            }

            if (playerWeaponAnimator.GetCurrentAnimatorStateInfo(0).length > playerWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime
                && playerWeaponAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TempSwordAnimation"
                && collision.gameObject.tag == "Weapon")
            {
                triggered = true;
                currentHealth -= PlayerStatManager.Instance.MeleeDamage.GetAppliedTotal();
                //Rigidbody enemyRb = gameObject.GetComponent<Rigidbody>();
                //if (enemyRb != null)
                //{
                //    // Calculate the knockback direction (away from the player)
                //    Vector3 knockbackDirection = collision.transform.position - transform.position;
                //    knockbackDirection.y = 0;  // Keep knockback horizontal (no vertical force)

                //    // Apply the knockback force
                //    float knockbackForce = 50f; // Adjust this value based on desired knockback strength
                //    enemyRb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
                //}
            }
        }

        IEnumerator SmoothTurn(float angle, float duration)
        {
            isRotating = true;

            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

            float time = 0;
            while (time < duration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;

            isRotating = false;
        }
    }
}
