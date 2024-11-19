using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        GameObject player;
        public StatManager stats;
        public List<Action> attackPattern;
        public EnemyState enemyState;
        public Animator animator;

        float distanceBetweenPlayer;
        public float attackDistance;
        float idleTime;
        public float attackCooldown; // Cooldown duration in seconds
        private float lastAttackTime = -Mathf.Infinity;

        protected virtual void Awake()
        {
            stats = new StatManager();
            attackPattern = new List<Action>();
            setState(EnemyState.IDLE);
            idleTime = 0f;
        }

        protected virtual void Start()
        {
            player = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject");
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
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
        public void setState(EnemyState state) { enemyState = state; } 
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
                // Rotate toward the target
                animator.SetBool("isMoving", true);
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
                animator.SetBool("isMoving", false);
                yield return null; // Wait for the next frame
            }
        }


        public virtual void idle()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                setState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal())
            {
                setState(EnemyState.TRIGGERED);
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
                setState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
            {
                setState(EnemyState.IDLE);
            }
            else if (distanceBetweenPlayer <= attackDistance)
            {
                setState(EnemyState.ATTACK);
            }
            else
            {
                animator.SetBool("isMoving", true);
                StopAllCoroutines();
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public virtual void attack()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                setState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer > attackDistance)
            {
                setState(EnemyState.IDLE);
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
    } 
}
