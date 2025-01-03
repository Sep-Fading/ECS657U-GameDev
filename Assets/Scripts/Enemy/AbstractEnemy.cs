using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using InventoryScripts;
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
        public AudioSource audioSource;

        public float distanceBetweenPlayer;
        public float attackDistance;
        public float idleTime;
        public float attackCooldown; // Cooldown duration in seconds
        public float lastAttackTime = -Mathf.Infinity;
        public bool isAttackComplete = false;
        public float xpDrop;
        public int goldDrop;

        public float baseSpeed;
        public float runSpeed;

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
            if (GetComponent<AudioSource>() != null) { audioSource = GetComponent<AudioSource>(); }
            animator.SetBool("isMoving", false);
            stats.Speed.SetFlat(baseSpeed);
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
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

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

            Vector3 lastPosition = transform.position;
            float stuckCheckInterval = 0.5f;
            float stuckThreshold = 0.05f;
            float elapsedTime = 0f;

            while (Vector3.Distance(transform.position, targetPosition) > stopDistance)
            {
                animator.SetBool("isMoving", true);
                // Rotate toward the target
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * stats.Speed.GetCurrent()
                );

                // Move toward the target
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    stats.Speed.GetCurrent() * Time.deltaTime
                );

                // Increment elapsed time
                elapsedTime += Time.deltaTime;

                // Check for stuck condition
                if (elapsedTime >= stuckCheckInterval)
                {
                    float distanceMoved = Vector3.Distance(transform.position, lastPosition);
                    if (distanceMoved <= stuckThreshold)
                    {
                        if (audioSource.isPlaying) { audioSource.Pause(); }
                        animator.SetBool("isMoving", false);
                        SetState(EnemyState.IDLE);
                        yield break; // Stop moving
                    }

                    // Reset for the next interval
                    lastPosition = transform.position;
                    elapsedTime = 0f;
                }

                yield return null; // Wait for the next frame
            }
            if (audioSource.isPlaying) { audioSource.Pause(); }
            animator.SetBool("isMoving", false);
        }
        public virtual void idle()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.TRIGGERED);

            else
            {
                stats.Speed.SetCurrent(baseSpeed);
                animator.SetBool("isMoving", false);
                if (idleTime <= 0)
                {
                    // Randomly decide between moving or staying still
                    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                    if (willMove)
                    {
                        audioSource.spatialBlend = 1f;
                        audioSource.loop = true;
                        audioSource.clip = Resources.Load("Audio/Level1Walk") as AudioClip;
                        if (!audioSource.isPlaying) { audioSource.Play(); }
                        animator.SetBool("isMoving", true);
                        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                        randomDirection += transform.position; // Offset by current position
                        randomDirection.y = transform.position.y; // Maintain current Y position

                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    else { audioSource.Pause(); }
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
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.IDLE);
            else if (distanceBetweenPlayer <= attackDistance) SetState(EnemyState.ATTACK);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", true);
                setSpeed(runSpeed);
                audioSource.spatialBlend = 1f;
                audioSource.loop = true;
                audioSource.clip = Resources.Load("Audio/Level1Run") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public virtual void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", false);

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
                        int attack = Random.Range(0, attackPattern.Count);
                        attackPattern[attack].Invoke();
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
        public virtual void deathAudio()
        {
            audioSource.spatialBlend = 0f;
            audioSource.loop = false;
            audioSource.clip = Resources.Load("Audio/EnemyDeath") as AudioClip;
            if (!audioSource.isPlaying) { audioSource.Play(); }
        }
        public virtual void destroySelf()
        {
            Debug.Log("Enemy Dead");
            if (XpManager.Instance != null) { XpManager.GiveXp(xpDrop); }
            if (Inventory.Instance != null) { Inventory.GiveGold(goldDrop); }
            Destroy(gameObject);
        }
        public void setSpeed(float speed) { stats.Speed.SetFlat((float) speed); }
        public virtual void onAttack()
        {
            isAttackComplete = true;
            foreach (var enemyWeapon in GetComponentsInChildren<EnemyWeapon>()) enemyWeapon.collider.enabled = true;
            if (playerStats != null && !playerStats.IsBlocking && GameObject.FindGameObjectWithTag("Shield") != null)
            {
                GameObject.FindGameObjectWithTag("Shield").GetComponent<Collider>().enabled = false;
            }
            //Debug.Log("Animation End");
            if ((player.GetComponent<InputManager>().getPlayerInput().grounded.ShieldAction.triggered && GameObject.FindWithTag("Shield") != null) || (playerStats.IsBlocking && player.GetComponent<InputManager>().getPlayerInput().grounded.ShieldAction.triggered && GameObject.FindWithTag("Weapon") != null))
            {
                Debug.Log("Parrying");
                animator.SetTrigger("stunTrigger");
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 4f, ForceMode.Impulse);
                StopAllCoroutines();
                setSpeed(0f);
                attackCooldown = 5f;

            }
        }
        public virtual void onAttackComplete()
        {
            setSpeed(runSpeed);
            SetState(EnemyState.TRIGGERED);
        }
    } 
}
