using Enemy;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace enemy
{
    public class OrcBossController : AbstractEnemy
    {
        bool isThrowing;
        protected override void Awake()
        {
            base.Awake();

            attackDistance = 3.5f;
            attackCooldown = 3f;
            attackPattern.Add(jumpAttack);
            //attackPattern.Add(weaponAttack);
            //attackPattern.Add(punchAttack);
            //attackPattern.Add(throwAttack);
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 7f;
            base.Start();
        }
        protected override void Update()
        {
            base.Update();

            if ((GetState() == EnemyState.TRIGGERED || GetState() == EnemyState.ATTACK) 
                && player.GetComponent<InputManager>().getPlayerInput().grounded.SwordAction.triggered 
                && Random.value < 0.3
                && !animator.GetAnimatorTransitionInfo(0).IsName("Dodge")
                && GameObject.FindWithTag("WeaponSlot").transform.childCount > 0)
            {
                animator.SetTrigger("dodgeTrigger");
                Debug.Log("Dodging");
            }
            //Debug.Log("Speed: " + stats.Speed.GetCurrent());
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
                    else
                    {
                        //bool willJump = Random.value > 0.5f;
                        //if (willJump) animator.SetTrigger("jumpTrigger");
                        //else animator.SetBool("isWalking", false);
                        animator.SetBool("isWalking", false);
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
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.IDLE);
            else if (distanceBetweenPlayer <= attackDistance)
            {
                SetState(EnemyState.ATTACK);
            }
            else
            {
                StopAllCoroutines();
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                StartCoroutine(MoveTo(player.transform.position));
                //animator.SetTrigger("jumpTrigger");
            }
        }
        public void punchAttack() { animator.SetTrigger("punchTrigger"); }
        public void weaponAttack() { animator.SetTrigger("weaponTrigger"); }
        public void throwAttack() 
        {
            animator.SetTrigger("weaponTrigger");
            isThrowing = true;
        }
        public void checkThrowAttack()
        {
            if (isThrowing)
            {
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = false;
                GameObject newWeapon = Instantiate(Resources.Load("Orc_Skull_Weapon"), transform) as GameObject;
                newWeapon.GetComponent<Renderer>().enabled = true;
                newWeapon.GetComponent<Rigidbody>().isKinematic = false;
                newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 30f, ForceMode.Impulse);
            }
        }
        public void enableWeapon()
        {
            if (isThrowing)
            {
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = true;
                //Destroy(GameObject.FindWithTag("EnemyWeaponThrowable"));
                GameObject newWeapon = Instantiate(Resources.Load("Orc_Skull_Weapon"), transform) as GameObject;
                isThrowing = false;
            }
        }
        public void jump() 
        {
            setSpeed(0f);
            Debug.Log((transform.position - player.transform.position).normalized);
            gameObject.GetComponent<Rigidbody>().AddForce((Vector3.up + (transform.position - player.transform.position).normalized) * 10f, ForceMode.Impulse);
        }
        public void jumpAttack()
        {
            animator.SetTrigger("jumpTrigger");
        }
        public void onJump()
        {
            animator.SetTrigger("jumpIdleTrigger");
        }
        public void checkNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4f)) 
            { 
                animator.SetTrigger("jumpEndTrigger");
                setSpeed(runSpeed); 
            }
        }
    }
}
