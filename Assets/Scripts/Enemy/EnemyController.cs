using System;
using System.Collections;
using System.Collections.Generic;
using GameplayMechanics.Character;
using NUnit.Framework;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// This handles how the enemies function, controlling their movement and also how they target the player
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        private static readonly int Color1 = Shader.PropertyToID("_Color");
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
        public Animator playerWeaponAnimator;
        public Material triggeredMaterial;
        public Material defaultMaterial;
        Renderer enemyRenderer;
        public TextMeshProUGUI healthText;
        private MaterialPropertyBlock propBlock;
        
        private StatManager _statManager;

        // Start is called before the first frame update
        void Start()
        {
            speed = 4f;
            triggered = false;
            //enemyRenderer = GetComponent<Renderer>();
            randomRot = UnityEngine.Random.Range(-360f, 360f);
            _statManager = new StatManager();
        }

        private void Awake()
        {
            
            player = GameObject.FindGameObjectWithTag("Player");
            playerWeaponAnimator = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<Animator>();
            

            if (player != null)
            {
                playerTransform = player.transform;
                playerMotor = player.GetComponent<PlayerMotor>();
            }
            
            enemyRenderer = GetComponent<Renderer>();
            enemyRenderer.material = defaultMaterial;
            wallCollider = GameObject.FindWithTag("Environment").GetComponent<Collider>();

            propBlock = new MaterialPropertyBlock();
        }

        // Update is called once per frame
        void Update()
        {
            if (_statManager.Life.GetCurrent() <= 0)
            {
                DefeatEnemy();
            }

            Vector3 playerPos = playerTransform.position;
            float distance = Vector3.Distance(transform.position, playerPos);
            bool obstacleBetween = Physics.Raycast(transform.position, (playerPos - transform.position).normalized, distance, obstacleLayer);
            healthText.text = _statManager.Life.GetCurrent() + "/" + _statManager.Life.GetAppliedTotal();

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
            /* --- DONT USE .material CALL AS IT MAKES BACKGROUND COPIES OF THE MATERIAL LEADING TO MEMORY LEAK ---*/
            enemyRenderer.GetPropertyBlock(propBlock);
            Color healthColor = triggered ? triggeredMaterial.color : defaultMaterial.color;
            healthColor.a = _statManager.Life.GetCurrent() / _statManager.Life.GetAppliedTotal();
            propBlock.SetColor(Color1, healthColor);
            enemyRenderer.SetPropertyBlock(propBlock);

            if (triggered)
            {
                if (inChaseRange)
                {
                    TrackPlayer(playerPos);
                }
                else if (inAttackRange)
                {
                    Attack();
                }
            }
            else
            {
                Idle();
            }

            triggered = inChaseRange || inAttackRange;
        }

        void Idle()
        {
            triggered = false;
            idleTime += Time.deltaTime;
            speed = 1f;
            if (!isRotating)
            {
                StartCoroutine(SmoothTurn(UnityEngine.Random.Range(-360f, 360f), 5f));
            }
            else if (idleTime > 2f)
            {
                idleTime = 0f;
            }
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        void TrackPlayer(Vector3 playerPos) 
        {
            speed = 4.5f;
            isRotating = false;
            triggered = true;
            transform.LookAt(playerPos);
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        void Attack() 
        { 
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                PlayerStatManager.Instance.TakeDamage(15f);
                attackCooldown = 1f;
            }
        }

        void DefeatEnemy() 
        {
            XpManager.GiveXp(20f);
            Destroy(gameObject);
        }

        public void DestroyEnemy() //does not give XP to player, just removes the object
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall") && !isRotating)
            {
                StartCoroutine(SmoothTurn(UnityEngine.Random.Range(90f, 180f), 1f));
            }

            if (playerWeaponAnimator.GetCurrentAnimatorStateInfo(0).length > playerWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime
                && playerWeaponAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "TempSwordAnimation"
                && collision.gameObject.CompareTag("Weapon"))
            {
                triggered = true;
                //PlayerStatManager.Instance.DoDamage(this);
                //currentHealth -= PlayerStatManager.Instance.MeleeDamage.GetAppliedTotal();
                
                
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
        
        public StatManager GetStatManager() => _statManager;

        public void setStatManager(StatManager statManager)
        {
           _statManager = statManager;
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
