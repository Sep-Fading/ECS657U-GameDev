using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Enemy
{
    public class FrogController : AbstractEnemy
    {
        protected override void Awake()
        {
            base.Awake();

            attackDistance = 2.5f;
            attackCooldown = 1f;
            attackPattern.Add(punchAttack);
            attackPattern.Add(weaponAttack);
            attackPattern.Add(jumpAttack);
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 6f;
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            if (animator.GetAnimatorTransitionInfo(0).IsName("Punch")
            || animator.GetAnimatorTransitionInfo(0).IsName("Weapon")
            || animator.GetAnimatorTransitionInfo(0).IsName("Stun")
            || animator.GetAnimatorTransitionInfo(0).IsName("Shoot"))
                setSpeed(0f);
            if (distanceBetweenPlayer < attackDistance) jumpAttack();
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
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            //else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.TRIGGERED);
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
                            Debug.Log("Attack: " + attack.Method);
                        }
                    }
                    else
                    {
                        int attack = Random.Range(0, attackPattern.Count);
                        attackPattern[attack].Invoke();
                        Debug.Log("Attack: " + attackPattern[attack].Method);
                    }
                    lastAttackTime = Time.time; // Reset cooldown
                }
                if (Random.value < 0.4f)
                { jumpAttack(); }
                SetState(EnemyState.TRIGGERED);
            }
        }
        public void punchAttack()
        {
            animator.SetTrigger("punchTrigger");
        }
        public void weaponAttack()
        {
            animator.SetTrigger("weaponTrigger");
        }
        public void shootAttack()
        {
            animator.SetTrigger("shootTrigger");
        }
        public void generateMudBall()
        {
            transform.LookAt(player.transform);
            GameObject newWeapon = Instantiate(Resources.Load("MudBall"), transform) as GameObject;
            newWeapon.GetComponent<Renderer>().enabled = true;
            newWeapon.GetComponent<Rigidbody>().isKinematic = false;
            newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 10f, ForceMode.Impulse);
        }
        public void jump()
        {
            setSpeed(0f);
            Debug.Log((transform.position - player.transform.position).normalized);
            gameObject.GetComponent<Rigidbody>().AddForce((Vector3.up + (transform.position - player.transform.position).normalized) * 7f, ForceMode.Impulse);
        }
        public void jumpAttack()
        {
            transform.LookAt(player.transform);
            animator.SetTrigger("jumpTrigger");
        }
        public void onJump()
        {
            animator.SetTrigger("jumpIdleTrigger");
        }
        public void checkNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                animator.SetTrigger("jumpEndTrigger");
                setSpeed(runSpeed);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon")
                //&& !(animator.GetAnimatorTransitionInfo(0).IsName("Punch") || animator.GetAnimatorTransitionInfo(0).IsName("Weapon")) 
                //&& GameObject.FindWithTag("WeaponHolder").GetComponent<Animator>().GetAnimatorTransitionInfo(0).IsName("TempSwordAnimation"))
                )
            {
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
            }
        }
    }
}
