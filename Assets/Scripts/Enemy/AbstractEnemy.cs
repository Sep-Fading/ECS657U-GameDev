using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbstractEnemy : MonoBehaviour
{
    GameObject player;
    StatManager stats;
    List<Action> attackPattern;
    EnemyState enemyState;
    float idleTime;
    Animator animator;
    float distanceBetweenPlayer;
    public float attackDistance;

    protected virtual void Awake()
    {
        attackDistance = 2.5f;
        stats = new StatManager();
        enemyState = EnemyState.Idle;
        idleTime = 0f;
    }

    private void Start()
    {
        player = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject");
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (enemyState)
        {
            case EnemyState.Idle:
                idle();
                break;
            case EnemyState.Triggered:
                followPlayer();
                break;
            case EnemyState.Attack: 
                attack(); 
                break;
            case EnemyState.Dead:
                despawn(); 
                break;
            default:
                break;
        }
    }
    IEnumerator MoveTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position.normalized;

        float minDistance = 0.1f; // Default stop distance
        if (enemyState == EnemyState.Triggered)
        {
            minDistance = attackDistance; // Stop at attack range
        }

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Early exit if already in position
        if (Vector3.Distance(transform.position, targetPosition) <= minDistance)
        {
            yield break;
        }

        // Rotate toward the target first
        while (Vector3.Distance(transform.position, targetPosition) > minDistance)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.Speed.GetCurrent());
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.Speed.GetCurrent() * Time.deltaTime);
            yield return null;
        }
    }
    void idle() 
    { 
        if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal())
        {
            enemyState = EnemyState.Triggered;
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
                    // Set destination
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
    void followPlayer() 
    {
        if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
        {
            enemyState = EnemyState.Idle;
        }
        else if (distanceBetweenPlayer <= attackDistance)
        {
            enemyState = EnemyState.Attack;
        }
        else
        {
            animator.SetBool("isMoving", true);
            StopAllCoroutines();
            StartCoroutine(MoveTo(player.transform.position));
        }
    }
    void attack() 
    {
        if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
        {
            enemyState = EnemyState.Idle;
        }
        else
        {
            animator.SetTrigger("attackTrigger");
        }
    }
    void block() { }
    void despawn() { }
}
