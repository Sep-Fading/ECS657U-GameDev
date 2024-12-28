using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SkeleMageController : AbstractEnemy
    {
        public bool attacking;
        public float spellcastTime;
        protected override void Awake()
        {
            base.Awake();

            attackDistance = 15f;
            attackCooldown = 1f;
            stats.TriggeredDistance.SetFlat(30f);
            attackPattern.Add(shootAttack);
            attacking = false;
            spellcastTime = 0f;
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 7f;
            base.Start();
        }
        protected override void Update()
        {
            distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Moving: " + animator.GetBool("isMoving"));
            if (animator.GetAnimatorTransitionInfo(0).IsName("Spellcast")
                || animator.GetAnimatorTransitionInfo(0).IsName("Shoot")
                || animator.GetAnimatorTransitionInfo(0).IsName("Stun"))
                setSpeed(0f);
            if (!attacking)
            {
                stats.Damage.SetMultiplier(0f);
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
            else
            {
                transform.LookAt(player.transform);
                if (spellcastTime <= 0f) { animator.SetTrigger("shootTrigger"); }
                spellcastTime -= Time.deltaTime;
            }
            
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

            animator.SetBool("isMoving", false);
            if (GetState() == EnemyState.IDLE) animator.SetBool("isWalking", false);
            if (GetState() == EnemyState.TRIGGERED) animator.SetBool("isRunning", false);
        }
        public override void idle()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.TRIGGERED);
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

                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    else animator.SetBool("isWalking", false);
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
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
            { SetState(EnemyState.IDLE); attacking = false; }
            else if (distanceBetweenPlayer <= attackDistance) SetState(EnemyState.ATTACK);
            else
            {
                setSpeed(runSpeed);
                StopAllCoroutines();
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer >= stats.TriggeredDistance.GetCurrent()) SetState(EnemyState.IDLE);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", false);

                if (Time.time - lastAttackTime >= attackCooldown) // Check cooldown
                {
                    int attack = Random.Range(0, attackPattern.Count);
                    attackPattern[attack].Invoke();
                    lastAttackTime = Time.time; // Reset cooldown
                }
            }
        }
        public void shootAttack()
        {
            animator.SetTrigger("castTrigger");
            attacking = true;
            spellcastTime = Random.Range(1, 5);
            stats.Damage.SetMultiplier(spellcastTime);
        }
        public override void onAttack()
        {
            transform.LookAt(player.transform);
            GameObject newWeapon = Instantiate(Resources.Load("FireBall"), transform) as GameObject;
            newWeapon.GetComponent<Renderer>().enabled = true;
            newWeapon.GetComponent<Rigidbody>().isKinematic = false;
            newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 20f, ForceMode.Impulse);
            base.onAttack();
        }
        public override void onAttackComplete()
        {
            base.onAttackComplete();
            attacking = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                Debug.Log("Enemy Attacked");
                //PlayerStatManager.Instance.DoDamage(this);
                //PlayerStatManager.Instance.DoDamage(enemy);
                StopAllCoroutines();
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
                attacking = false;
            }
        }
    }
}
