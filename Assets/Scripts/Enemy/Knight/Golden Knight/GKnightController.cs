using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Enemy
{
    public class GKnightController : AbstractEnemy
    {
        public Vector3 spawnpoint;
        protected override void Awake()
        {
            base.Awake();
            xpDrop = 25f;
            goldDrop = 40;
            attackDistance = 5f;
            attackCooldown = 1f;
            attackPattern.Add(weaponAttack);
            attackPattern.Add(poundAttack);
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 6f;
            spawnpoint = transform.position;
            stats.TriggeredDistance.SetFlat(30f);
            stats.Life.SetFlat(200f);
            stats.Damage.SetFlat(20f);
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
            if (GetState() == EnemyState.IDLE) animator.SetBool("isWalking", false); transform.LookAt(player.transform);
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
                animator.SetBool("isWalking", true);
                Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                randomDirection += transform.position; // Offset by current position
                randomDirection.y = transform.position.y; // Maintain current Y position
                audioSource.spatialBlend = 1f;
                audioSource.loop = true;
                audioSource.clip = Resources.Load("Audio/KnightWalk") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                StopAllCoroutines();
                StartCoroutine(MoveTo(spawnpoint));
                //if (idleTime <= 0)
                //{
                //    // Randomly decide between moving or staying still
                //    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                //    if (willMove)
                //    {
                //        animator.SetBool("isWalking", true);
                //        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                //        randomDirection += transform.position; // Offset by current position
                //        randomDirection.y = transform.position.y; // Maintain current Y position

                //        StopAllCoroutines();
                //        StartCoroutine(MoveTo(randomDirection));
                //    }
                //    else animator.SetBool("isWalking", false);
                //    // Set a new idle duration (2-5 seconds)
                //    idleTime = Random.Range(2f, 5f);
                //}
                //else
                //{
                //    idleTime -= Time.deltaTime; // Count down idle time
                //}
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
                audioSource.clip = Resources.Load("Audio/KnightRun") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public void weaponAttack()
        {
            animator.SetTrigger("attackTrigger");
        }
        public void poundAttack()
        {
            animator.SetTrigger("poundTrigger");
        }
        public override void onAttack()
        {
            isAttackComplete = true;
            //GetComponentInChildren<EnemyWeapon>().collider.enabled = true;
            foreach (var enemyWeapon in GetComponentsInChildren<EnemyWeapon>()) enemyWeapon.collider.enabled = true;
            if (playerStats != null && GameObject.FindWithTag("Shield") != null && player.GetComponent<InputManager>().getPlayerInput().grounded.ShieldAction.triggered)
            {
                Debug.Log("Parrying");
                animator.SetTrigger("stunTrigger");
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back) * 4f, ForceMode.Impulse);
                StopAllCoroutines();
                setSpeed(0f);
                attackCooldown = 5f;
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/Parry") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
            else
            {
                if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanArmature|Run_swordAttack")
                {
                    if (playerStats != null && !playerStats.IsBlocking && GameObject.FindGameObjectWithTag("Shield") != null)
                    {
                        GameObject.FindGameObjectWithTag("Shield").GetComponent<Collider>().enabled = false;
                        playerStats.TakeDamage(stats.Damage.GetCurrent());
                    }
                }
                else if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanArmature|swordAttackJump")
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackDistance);
                    foreach (var hitCollider in hitColliders)
                    {
                        //Debug.Log(hitColliders);
                        if (hitCollider.gameObject.CompareTag("Player"))
                        {
                            player.GetComponent<CharacterController>().Move(player.transform.position - transform.position);
                            playerStats.TakeDamage(stats.Damage.GetCurrent());
                            Debug.Log("Player Pounded");
                        }
                    }
                }
            }
        }
        public override void destroySelf()
        {
            if (GameObject.Find("BossGate") != null)
            {
                GameObject.Find("BossGate").GetComponent<Collider>().enabled = true;
            }
            base.destroySelf();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                setSpeed(0f);
                playerStats.DoDamage(this);
                animator.SetTrigger("stunTrigger");
                StopAllCoroutines();
                attackCooldown = 1f;
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back) * 2f, ForceMode.Impulse);
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/KnightHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
    }
}
