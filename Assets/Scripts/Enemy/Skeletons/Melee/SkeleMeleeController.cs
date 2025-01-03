using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SkeleMeleeController : AbstractEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            xpDrop = 25f;
            goldDrop = 35;
            attackDistance = 3f;
            attackCooldown = 1f;
            attackPattern.Add(chopAttack);
            attackPattern.Add(diagAttack);
            attackPattern.Add(jumpAttack);
            attackPattern.Add(horiAttack);
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 8f;
            stats.Life.SetFlat(150f);
            stats.Damage.SetFlat(15f);
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
        }
        public override IEnumerator MoveTo(Vector3 targetPosition)
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
                if (GetState() == EnemyState.IDLE) animator.SetBool("isWalking", true);
                if (GetState() == EnemyState.TRIGGERED) animator.SetBool("isRunning", true);

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
                        animator.SetBool("isWalking", false);
                        animator.SetBool("isRunning", false);
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
            if (GetState() == EnemyState.IDLE) animator.SetBool("isWalking", false);
            if (GetState() == EnemyState.TRIGGERED) animator.SetBool("isRunning", false);
        }
        public override void idle()
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
                setSpeed(baseSpeed);    
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                if (idleTime <= 0)
                {
                    // Randomly decide between moving or staying still
                    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                    if (willMove)
                    {
                        animator.SetBool("isWalking", true);
                        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                        randomDirection += transform.position; // Offset by current position
                        randomDirection.y = transform.position.y; // Maintain current Y position
                        audioSource.spatialBlend = 1f;
                        audioSource.loop = true;
                        audioSource.clip = Resources.Load("Audio/SkeletonWalk") as AudioClip;
                        if (!audioSource.isPlaying) { audioSource.Play(); }
                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    else
                    {
                        animator.SetBool("isWalking", false);
                        audioSource.Pause();
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
        public override void followPlayer()
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
                setSpeed(runSpeed);
                StopAllCoroutines();
                audioSource.spatialBlend = 1f;
                audioSource.loop = true;
                audioSource.clip = Resources.Load("Audio/SkeletonRun") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", false);

                if (Time.time - lastAttackTime >= attackCooldown) // Check cooldown
                {
                    transform.LookAt(player.transform);
                    int attack = Random.Range(0, attackPattern.Count);
                    attackPattern[attack].Invoke();
                    lastAttackTime = Time.time; // Reset cooldown
                }
            }
        }
        public void chopAttack()
        {
            animator.SetTrigger("chopTrigger");
        }
        public void diagAttack() 
        {
            animator.SetTrigger("diagTrigger");
        }
        public void horiAttack()
        {
            animator.SetTrigger("horiTrigger");
        }
        public void jumpAttack()
        {
            animator.SetTrigger("jumpAttackTrigger");
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                playerStats.DoDamage(this);
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back) * 2f, ForceMode.Impulse);
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/EnemyHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
    }
}
