using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

namespace Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        public GameObject player;
        public StatManager stats;
        public PlayerStatManager playerStats;
        public List<Action> attackPattern;
        public EnemyState enemyState;
        public Animator animator;

        public float distanceBetweenPlayer;
        public float attackDistance;
        public float idleTime;
        public float attackCooldown; // Cooldown duration in seconds
        public float lastAttackTime = -Mathf.Infinity;
        public bool isAttackComplete = false;

        protected virtual void Awake()
        {
            stats = new StatManager();
            attackPattern = new List<Action>();
            SetState(EnemyState.IDLE);
            idleTime = 0f;
        }

        protected virtual void Start()
        {
            player = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject");
            playerStats = PlayerStatManager.Instance;
            animator = GetComponent<Animator>();
            animator.SetBool("isMoving", false);
        }

        protected virtual void Update()
        {
            distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Moving: " + animator.GetBool("isMoving"));
            switch (enemyState)
            {
                case EnemyState.IDLE:
                    idle();
                    break;
                case EnemyState.TRIGGERED:
                    followPlayer();
                    break;
                case EnemyState.ATTACK:
                    attack();
                    break;
                case EnemyState.DEAD:
                    despawn();
                    break;
                default:
                    break;
            }
        }
        public StatManager GetStatManager() => stats;
        public EnemyState GetState() { return enemyState; }
        public void SetState(EnemyState state) { enemyState = state; } 
        public virtual IEnumerator MoveTo(Vector3 targetPosition)
        {
            // Calculate the direction to the target
            Vector3 direction = targetPosition - transform.position;

            if (direction.magnitude <= 0.001f)
            {
                yield break; // Exit if the target position is too close
            }

            // Normalize the direction
            direction.Normalize();

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float stopDistance = GetState() == EnemyState.TRIGGERED ? attackDistance : 0.1f;

            while (Vector3.Distance(transform.position, targetPosition) > stopDistance)
            {
                animator.SetBool("isMoving", true);
                // Rotate toward the target
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * stats.Speed.GetCurrent()
                );

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    stats.Speed.GetCurrent() * Time.deltaTime
                );
                yield return null; // Wait for the next frame
            }
            animator.SetBool("isMoving", false);
        }
        public virtual void idle()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal())
            {
                SetState(EnemyState.TRIGGERED);
            }
            else
            {
                animator.SetBool("isMoving", false);
                if (idleTime <= 0)
                {
                    // Randomly decide between moving or staying still
                    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                    if (willMove)
                    {
                        animator.SetBool("isMoving", true);
                        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                        randomDirection += transform.position; // Offset by current position
                        randomDirection.y = transform.position.y; // Maintain current Y position

                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    // Set a new idle duration (2-5 seconds)
                    idleTime = Random.Range(2f, 5f);
                }
                else
                {
                    idleTime -= Time.deltaTime; // Count down idle time
                }
            }
        }
        public virtual void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
            {
                SetState(EnemyState.IDLE);
            }
            else if (distanceBetweenPlayer <= attackDistance)
            {
                SetState(EnemyState.ATTACK);
            }
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", true);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public virtual void attack()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer > attackDistance)
            {
                SetState(EnemyState.IDLE);
            }
            else
            {
                if (Time.time - lastAttackTime >= attackCooldown) // Check cooldown
                {
                    bool runPattern = Random.value > 0.5f; // 50% chance to run attack pattern or a random attack
                    if (runPattern)
                    {
                        foreach (var attack in attackPattern)
                        {
                            attack.Invoke();
                        }
                    }
                    else
                    {
                        attackPattern[Random.Range(0, attackPattern.Count)].Invoke();
                    }
                    lastAttackTime = Time.time; // Reset cooldown
                }
            }
        }
        public virtual void block() { }
        public virtual void despawn()
        {
            animator.SetTrigger("deathTrigger");
        }
        public void destroySelf()
        {
            Debug.Log("Enemy Dead");
            Destroy(gameObject);
        }
        public void onAttack()
        {
            isAttackComplete = true;
            //GetComponentInChildren<EnemyWeapon>().collider.enabled = true;
            foreach (var enemyWeapon in GetComponentsInChildren<EnemyWeapon>()) enemyWeapon.collider.enabled = true;
            if (playerStats != null && !playerStats.IsBlocking) GameObject.FindGameObjectWithTag("ShieldSlot").GetComponentInChildren<CapsuleCollider>().enabled = false;
            //Debug.Log("Animation End");
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (GetState() == EnemyState.ATTACK && !playerStats.IsBlocking && collision.gameObject.tag == "Player")
            {
                
            }
        }
    } 
}
